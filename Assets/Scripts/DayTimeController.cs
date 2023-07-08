
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{

    public float secondsInADay;
    public float secondsInAnHour;

    public Color nightLightColor;
    public AnimationCurve nightTimeCurve;
    public Color dayLightColor = Color.white;
    public GameObject dayTransitionOverlay;


    float time = 0;
    public bool isTimePaused = false;
    int currentDay = 0;

    public TMP_Text currTime;
    //TODO can plug in lighting here

    void Update()
    {
        if(!isTimePaused)
        {
            time += Time.deltaTime;
        }
        int numDays = (int)(time / secondsInADay);
        if( numDays > currentDay)
        {
            currentDay = numDays;
            StartCoroutine(DayTransition());
        }
        float numSecRemaining = time % secondsInADay;
        int numHours = (int)(numSecRemaining / secondsInAnHour);
        currTime.text = "Day: " + (numDays+1).ToString() + "\nTime: " + numHours.ToString() + ":00\n" + "elapsedTime: " + time.ToString();
    }

    private IEnumerator DayTransition()
    {
        togglePausedTime();
        float fadeTime = 3f;
        float waitTime = 3f;
        float elapsedTime = 0f;
        dayTransitionOverlay.SetActive(true);

        PlayerManager playerManager= GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerManager>();
        int hunger = playerManager.hunger;
        int ap = playerManager.ap;

        var textFields = dayTransitionOverlay.GetComponentsInChildren<TMP_Text>();
        int dayNumberTextIndex = 0;
        int apGainedText = 1;
        textFields[dayNumberTextIndex].text = "Day " + (currentDay + 1).ToString();
        textFields[apGainedText].text = "Hunger was " + hunger.ToString() + "\nAp gained is " + hunger.ToString();

        playerManager.ChangeAp(hunger);

        //fade in
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            dayTransitionOverlay.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, elapsedTime / fadeTime);
            yield return null;
        }
        elapsedTime = 0f;
        yield return new WaitForSeconds(waitTime);

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            dayTransitionOverlay.GetComponent<CanvasGroup>().alpha = Mathf.Lerp( 1, 0, elapsedTime / fadeTime);
            yield return null;
        }
        //yield return new WaitForSeconds(waitTime);
        dayTransitionOverlay.SetActive(false);
        togglePausedTime();
    }

    public void togglePausedTime()
    {
        isTimePaused = !isTimePaused;
    }

    public void SetPausedTime(bool paused)
    {
        isTimePaused = paused;
    }

    public float getCurrentTimeSeconds()
    {
        return time;
    }
}
