
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class DayTimeController : MonoBehaviour
{

    public static float secondsInADay = 32;
    public static float secondsInAnHour = 2;
    // hoursInADay = 16

    public Color nightLightColor;
    public AnimationCurve nightTimeCurve;
    public Color dayLightColor = Color.white;
    public OverlayManager overlayManager;
    public PlayerMovement playerMovement;
    public GameObject pauseButtonObject;
    public GameObject pauseOverlay;
    public int numDaysToWin;

    float time = 0;
    public bool isTimePaused = false;
    int currentDay = 0;

    private Button pauseButtonElement;

    public TMP_Text currTime;
    //TODO can plug in lighting here

    public void Start()
    {
        pauseButtonElement = pauseButtonObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("pauseButton");
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
            pauseButtonObject.SetActive(false);
            overlayManager.GetComponent<OverlayManager>().GameOverTransition(
                "You Did It.\nYou Survived",
                "Despite all of the odds, you, a humble goblin farmer living between two warring factions have survived the war. Were you able to live proudly? Or did you need to do what it took to survive. Maybe for another goblin, the conditions would have been different.",
                true,
                "Another Story Awaits",
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
        //int numMinutes = (int)time % (int)secondsInAnHour * 15;
        //string numMinutesText = numMinutes.ToString();
        //if (numMinutes < 10)
        //{
        //    numMinutesText = "0" + numMinutesText;
        //}
        currTime.text = "Day " + (numDays + 1).ToString() + "\n" + numHoursText + ":00"; //+ "elapsedTime: " + time.ToString();

        updatePauseButtonOnClick();
    }

    public void updatePauseButtonOnClick() {
        pauseButtonElement.clicked += () =>
        {
            if (isTimePaused)
            {
                SetPausedTime(false);
                pauseButtonElement.text = "Pause";
            }
            else
            {
                SetPausedTime(true);
                pauseButtonElement.text = "Resume";
            }
        };
    }

    public void SetPausedTime(bool paused)
    {
        isTimePaused = paused;
        pauseOverlay.SetActive(paused);
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
