using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{

    public float maturationTime;
    public float waterDurationTime;
    public float dryToleranceTime;
    public Yield yield;
    public DayTimeController dayTimeController;
    // Start is called before the first frame update
    void Start()
    {
        dayTimeController = GameObject.FindObjectOfType<DayTimeController>();
        maturationTime = dayTimeController.secondsInADay;
        dryToleranceTime = dayTimeController.secondsInAnHour;
        waterDurationTime = dayTimeController.secondsInAnHour * 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
