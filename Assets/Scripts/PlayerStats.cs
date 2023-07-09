using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PlayerStats : MonoBehaviour
{

    public int maxAp;
    public int maxHunger;
    public int hungerLostPerTick;
    public int hunger { set; get; }
    public int ap { set; get; }
    public GameObject hungerEmptyBar;
    public TMP_Text hungerText;
    public GameObject apEmptyBar;
    public TMP_Text apText;
    public GameObject overlayManager;

    float nextHungerTick;
    float hungerTickIntervalSeconds = DayTimeController.secondsInAnHour * 3; // -1 hunger every 3 hours
    DayTimeController dayTimeController;

    private EventManager eventManager;
    private int warningHunger = 3;

    // Start is called before the first frame update
    public void Start()
    {
        hunger = maxHunger / 3; // Start at 3/10 hunger so the player learns to eat
        SetBar(BarType.Hunger, .33f);
        ap = maxAp / 2; // Start at 7/15 energy so the player learns to eat
        SetBar(BarType.AP, 0.5f);
        nextHungerTick = hungerTickIntervalSeconds;
        dayTimeController = FindAnyObjectByType<DayTimeController>();

        eventManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
        eventManager.PrintResultAfterDelay(1f, "You feel hungry, check your inventory (I) for something to eat.");
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
        if (ap > maxAp) ap = maxAp;
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
        return this.ap >= ap;
    }

    public void ChangeHunger(int delta)
    {
        hunger += delta;
        if (hunger > maxHunger) hunger = maxHunger;
        if (hunger <= warningHunger)
        {
            eventManager.PrintResult("You are starving (" + hunger + ")");
        }
        if (hunger <= 0)
        {
            overlayManager.GetComponent<OverlayManager>().GameOverTransition(
                "You Starved",
                "You died as you lived, hungry and alone. As you struggle to maintain consciousness you wonder to yourself... what could have happened if things went just a little differently?",
                true,
                "The Struggle Continues",
                // Restart manager will 
                () => FindAnyObjectByType<RestartManager>().Restart()
            );
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
                hungerText.text = hunger + "/" + maxHunger;
                break;
            case BarType.AP:
                apEmptyBar.GetComponent<Image>().fillAmount = emptyRatio;
                apText.text = ap+ "/" + maxAp;
                break;
        }
    }
}
