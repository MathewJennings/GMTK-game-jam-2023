using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : MonoBehaviour
{

    public PlayerStats playerStats;
    public DayTimeController dayTimeController;
    public EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Restart()
    {
        playerStats.Start();
        dayTimeController.Start();
        eventManager.Start();
    }
}
