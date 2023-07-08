
using TMPro;
using UnityEngine;

public class DayTimeController : MonoBehaviour
{

    public static float secondsInADay = 60;
    public static float secondsInAnHour = 6;

    public Color nightLightColor;
    public AnimationCurve nightTimeCurve;
    public Color dayLightColor = Color.white;


    float time = 0;
    public bool isTimePaused = false;

    public TMP_Text currTime;
    //TODO can plug in lighting here


    void Update()
    {
        if(!isTimePaused)
        {
            time += Time.deltaTime;
        }
        int numDays = (int)(time / secondsInADay);
        float numSecRemaining = time % secondsInADay;
        int numHours = (int)(numSecRemaining / secondsInAnHour);
        currTime.text = "Day: " + numDays.ToString() + "\nTime: " + numHours.ToString() + ":00\n" + "elapsedTime: " + time.ToString();
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
