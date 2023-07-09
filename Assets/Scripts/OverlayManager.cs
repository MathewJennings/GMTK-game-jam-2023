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
        inventoryUi = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DayTransition(int currentDay)
    {
        PlayerStats playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        int hunger = playerStats.hunger;
        StartCoroutine(DisplayOverlay("Day " + (currentDay + 1).ToString(), "Hunger was " + hunger.ToString() + "\nAp gained is " + hunger.ToString(), false)); 
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
                buttonDelegate,
                true, true)
        );
    }

    public IEnumerator DisplayOverlay(string mainText, string subText, bool showButton = false, string buttonText = "", ButtonDelegate buttonDelegate = null, bool showSummary = false, bool shouldFadeIn = true)
    {
        dayTimeController.SetPausedTime(true);
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
        if (shouldFadeIn)
        {
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                dayTransitionOverlay.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
        }

        //If we need to show the summary on a second screen, intercept the try again button and go to the summary screen first
        if(showSummary)
        {
            Button b = dayTransitionOverlay.GetComponentInChildren<Button>(true);
            b.gameObject.SetActive(true);
            b.GetComponentInChildren<TMP_Text>().text = "See Adventure Summary";
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => {
                StartCoroutine(
                    DisplayOverlay(
                        "Your Story",
                        FindAnyObjectByType<SummaryManager>().GenerateSummary(),
                        showButton,
                        buttonText,
                        buttonDelegate,
                        false, 
                        false)
                );
            });
        }
        //if waiting on button input
        else if (showButton)
        {
            Button b = dayTransitionOverlay.GetComponentInChildren<Button>(true);
            b.gameObject.SetActive(true);
            b.GetComponentInChildren<TMP_Text>().text = buttonText;
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => {
                buttonDelegate.Invoke();
                //resetting the daytimecontroller resumes time
                dayTimeController.SetPausedTime(true);
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
        dayTimeController.SetPausedTime(false);

    }
    public delegate void ButtonDelegate();
}
