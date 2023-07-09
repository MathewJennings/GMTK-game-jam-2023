
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{

    public static float secondsInADay = 64;
    public static float secondsInAnHour = 4;
    // hoursInADay = 16

    public Color nightLightColor;
    public AnimationCurve nightTimeCurve;
    public Color dayLightColor = Color.white;
    public OverlayManager overlayManager;
    public PlayerMovement playerMovement;
    public int numDaysToWin;

    float time = 0;
    public bool isTimePaused = false;
    int currentDay = 0;

    public TMP_Text currTime;
    //TODO can plug in lighting here

    public void Start()
    {
        time = 0;
        isTimePaused = false;
        currentDay = 0;
    }
    void Update()
    {
        if (isTimePaused)
        {
            return;
        } else if (!isTimePaused)
        {
            time += Time.deltaTime;
        }
        int numDays = (int)(time / secondsInADay);
        bool dayTransitioned = numDays > currentDay;
        if (dayTransitioned)
        {
            currentDay = numDays;
        }
        if (currentDay >= numDaysToWin)
        {
            overlayManager.GetComponent<OverlayManager>().GameOverTransition(
                "You Did It. You Survived",
                "Despite all of the odds, you, a humble goblin farmer living between two warring factions have survived the war. Were you able to live proudly? Or did you need to do what it took to survive. Maybe for another goblin, the conditions would have been different.",
                true,
                "Try Again",
                // Restart manager will 
                () => FindAnyObjectByType<RestartManager>().Restart()
            );
        } else if (dayTransitioned)
        {
            overlayManager.DayTransition(currentDay);
        }
        float numSecRemaining = time % secondsInADay;
        int numHours = (int)(numSecRemaining / secondsInAnHour) + 6; // Start day at 6am
        string numHoursText = numHours < 10 ? "0" + numHours : numHours.ToString();
        currTime.text = "Day: " + (numDays+1).ToString() + "\nTime: " + numHoursText + ":00\n" + "elapsedTime: " + time.ToString();
    }


    public void togglePausedTime()
    {
        isTimePaused = !isTimePaused;
    }

    public void SetPausedTime(bool paused)
    {
        isTimePaused = paused;
        playerMovement.setAllowMovement(!paused);
    }

    public float getCurrentTimeSeconds()
    {
        return time;
    }

    public int getCurrentDay()
    {
        return currentDay;
    }
}
