using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    public GameObject dayTransitionOverlay;
    DayTimeController dayTimeController;
    InventoryUI inventoryUi;

    float fadeTime = 3f;
    float waitTime = 3f;
    // Start is called before the first frame update
    void Start()
    {

        dayTimeController = FindAnyObjectByType<DayTimeController>();
        inventoryUi = GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DayTransition(int currentDay)
    {
        PlayerStats playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        int hunger = playerStats.hunger;
        StartCoroutine(DisplayOverlay("Day " + (currentDay + 1).ToString(), "Hunger was " + hunger.ToString() + "\nAp gained is " + hunger.ToString())); 
        playerStats.ChangeAp(hunger);
    }

    public void GameOverTransition(string title, string subtitle, bool showButton, string buttonText, ButtonDelegate buttonDelegate)
    {
        StartCoroutine(
            DisplayOverlay(
                title,
                subtitle,
                showButton,
                buttonText, 
                buttonDelegate
            )
        );
    }

    public IEnumerator DisplayOverlay(string mainText, string subText, bool showButton = false, string buttonText = "", ButtonDelegate buttonDelegate = null)
    {
        dayTimeController.togglePausedTime();
        float fadeTime = 3f;
        float waitTime = 3f;
        float elapsedTime = 0f;
        inventoryUi.CloseInventory();
        dayTransitionOverlay.SetActive(true);


        var textFields = dayTransitionOverlay.GetComponentsInChildren<TMP_Text>();
        int dayNumberTextIndex = 0;
        int apGainedText = 1;
        textFields[dayNumberTextIndex].text = mainText;
        textFields[apGainedText].text = subText;

        
        //fade in
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            dayTransitionOverlay.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            yield return null;
        }


        yield return new WaitForSeconds(waitTime);
        //if waiting on button input
        if (showButton)
        {
            Button b = dayTransitionOverlay.GetComponentInChildren<Button>(true);
            b.gameObject.SetActive(true);
            b.GetComponentInChildren<TMP_Text>().text = buttonText;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => {
                buttonDelegate.Invoke();
                b.gameObject.SetActive(false);
                //fade out
                StartCoroutine(FadeOut());
            });
        } else
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        //fade out
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            dayTransitionOverlay.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
            yield return null;
        }
        //yield return new WaitForSeconds(waitTime);
        dayTransitionOverlay.SetActive(false);
        dayTimeController.togglePausedTime();

    }
    public delegate void ButtonDelegate();
}
