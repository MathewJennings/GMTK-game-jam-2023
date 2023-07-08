using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public int maxAp;
    public int maxHunger;
    public float hungerTickIntervalSeconds;
    public int hungerLostPerTick;
    public int hunger { set; get; }
    public int ap { set; get; }
    public GameObject hungerEmptyBar;
    public GameObject apEmptyBar;

    float nextHungerTick;
    DayTimeController dayTimeController;

    // Start is called before the first frame update
    void Start()
    {
        ap = maxAp; 
        hunger = maxHunger;
        SetBar(BarType.Hunger, maxHunger);
        SetBar(BarType.AP, maxAp);
        nextHungerTick = hungerTickIntervalSeconds;
        dayTimeController = FindAnyObjectByType<DayTimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nextHungerTick <= dayTimeController.getCurrentTimeSeconds())
        {
            TickHunger();
        }
    }

    void TickHunger()
    {
        ChangeHunger(-hungerLostPerTick);
        nextHungerTick += hungerTickIntervalSeconds;
    }

    [ContextMenu("ReduceApBy5")]
    public void testReduceApBy5()
    {
        ChangeAp(-5);
    }

    public void ChangeAp(int delta)
    {
        ap += delta;
        if (ap <= 0)
        {
            Debug.Log("You are overworked. You died");
            ap = 0;
            SetBar(BarType.AP, 0);
        } else if (ap >= maxAp) {
            SetBar(BarType.AP, maxAp);
            ap = maxAp;
        } else
        {
            SetBar(BarType.AP, ((float)ap) / (float)maxAp);
        }
    }

    [ContextMenu("Reduce hunger by 5")]
    public void testReduceHungerBy5()
    {
        ChangeHunger(-5);
    }

    [ContextMenu("Increase hunger by 11")]
    public void testIncreaseHungerBy11()
    {
        ChangeHunger(11);
    }

    public bool canAffordAction(int ap)
    {
        return this.ap > ap;
    }

    public void ChangeHunger(int delta)
    {
        hunger += delta;
        if(hunger <= 0)
        {
            Debug.Log("You died as you lived, hungry and alone.");
            hunger = 0;
            SetBar(BarType.Hunger, 0);
        } else if(hunger >= maxHunger)
        {
            SetBar(BarType.Hunger, 1);
            hunger = maxHunger;
            nextHungerTick = dayTimeController.GetComponent<DayTimeController>().getCurrentTimeSeconds() + hungerTickIntervalSeconds;
        }
        else
        {
            SetBar(BarType.Hunger, ((float)hunger)/(float)maxHunger);
        }
    }

    enum BarType
    {
        Hunger,
        AP
    }
    void SetBar(BarType barType, float ratio)
    {
        //flip the ratio because we are controlling the empty bar
        float emptyRatio = 1f - ratio;
        switch (barType)
        {
            case BarType.Hunger:
                hungerEmptyBar.GetComponent<Image>().fillAmount = emptyRatio;
                break;
            case BarType.AP:
                apEmptyBar.GetComponent<Image>().fillAmount = emptyRatio;
                break;
        }
    }
}
