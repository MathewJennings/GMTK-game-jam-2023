using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{

    public float hoursToMaturation;
    public float waterDurationInHours;
    public float dryTimeToleranceInHours;
    public int yieldQuantity;
    public Item yield;
    public Sprite sproutSprite;

    public float getMaturationTime()
    {
        return hoursToMaturation * DayTimeController.secondsInAnHour;
    }

    public float getWaterDurationTime()
    {
        return waterDurationInHours * DayTimeController.secondsInAnHour;
    }

    public float getDryToleranceTime()
    {
        return dryTimeToleranceInHours * DayTimeController.secondsInAnHour;
    }
}
