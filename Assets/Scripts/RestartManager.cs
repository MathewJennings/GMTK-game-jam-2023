using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : MonoBehaviour
{

    public PlayerStats playerStats;
    public DayTimeController dayTimeController;
    public EventManager eventManager;
    public GameObject farmPlots;
    public Inventory playerInventory;
    public Inventory merchantInventory;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Restart()
    {
        playerStats.Start();
        dayTimeController.Start();
        eventManager.Start();
        //reset every plot
        foreach (Plot p in farmPlots.GetComponentsInChildren<Plot>())
        {
            p.Start();
        }
        playerInventory.Start();
        merchantInventory.Start();
    }
}
