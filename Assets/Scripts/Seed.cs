using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{

    public float maturationHours;
    public float waterHours;
    public float dryHours;
    public int yieldQuantity;
    public Item yield;
    public Sprite sproutSprite;

    public float getMaturationTime()
    {
        return maturationHours * DayTimeController.secondsInAnHour;
    }

    public float getWaterDurationTime()
    {
        return waterHours * DayTimeController.secondsInAnHour;
    }

    public float getDryToleranceTime()
    {
        return dryHours * DayTimeController.secondsInAnHour;
    }
}
