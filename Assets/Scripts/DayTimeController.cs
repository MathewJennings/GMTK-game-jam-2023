
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{

    public static float secondsInADay = 6;
    public static float secondsInAnHour = 2;

    public Color nightLightColor;
    public AnimationCurve nightTimeCurve;
    public Color dayLightColor = Color.white;
    public OverlayManager overlayManager;

    private InventoryUI playerInventoryUi;


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
        playerInventoryUi = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryUI>();
    }
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
            overlayManager.DayTransition(currentDay);
        }
        float numSecRemaining = time % secondsInADay;
        int numHours = (int)(numSecRemaining / secondsInAnHour);
        currTime.text = "Day: " + (numDays+1).ToString() + "\nTime: " + numHours.ToString() + ":00\n" + "elapsedTime: " + time.ToString();
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

    public int getCurrentDay()
    {
        return currentDay;
    }
}
