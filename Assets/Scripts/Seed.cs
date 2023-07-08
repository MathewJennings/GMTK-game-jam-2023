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
    private DayTimeController dayTimeController;

    // Start is called before the first frame update
    void Start()
    {
        dayTimeController = GameObject.FindObjectOfType<DayTimeController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float getMaturationTime()
    {
        return maturationHours * dayTimeController.secondsInAnHour;
    }

    public float getWaterDurationTime()
    {
        return waterHours * dayTimeController.secondsInAnHour;
    }

    public float getDryToleranceTime()
    {
        return dryHours * dayTimeController.secondsInAnHour;
    }
}
