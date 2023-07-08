using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    Queue<Event> events;
    Event nextEvent;
    public DayTimeController dayTimeController;
    public TMP_Text dialogText;
    public List<Button> choiceButtons;
    public GameObject dialogBox;
    public List<GameObject> allNpcPrefabsList;
    public GameObject npcManager;

    //when the next event that was added to the queue is
    private static float nextEventTime;

    // What day the EventManager thinks it currently is
    private float eventCurrentDay;

    private static GameObject npc;


    private static List<EventTemplate> eventTemplates;




    public static int human_loyalty =0 ;
    public static int goblin_loyalty = 0;
    public static int philanthropic = 0;
    public static int business = 0;

    public static int human_loyalty_guests_threshold = 3;
    public static int goblin_loyalty_guests_threshold = 3;
    public static int goblin_loyalty_kick_threshold = -3;

    public bool human_guests_occurred;
    public bool goblin_guests_occurred;
    public bool goblin_kick_occured;
  

// Start is called before the first frame update
public void Start()
    {
        eventTemplates = new List<EventTemplate> {
            new EventTemplate(
                "merchant",
                "You hear a knock at your gate. \"Would you like to make a trade?",
                new List<string> { "Let's Trade", "Ignore" },
                new List<EventDelegate> { openShopMenu, closeDialog },
                allNpcPrefabsList[0],1
            ),
            new EventTemplate(
                "lady", 
                "You hear a knock at your gate. \"Would you help me? I'm a poor defenseless villager and my child is sick. I need 5 gold to buy some medicine\". You ponder your options", 
                new List<string> { "Give gold!", "Rob her!" }, 
                new List<EventDelegate> { giveGrandma, robGrandma },
                allNpcPrefabsList[2],1
            ),
            new EventTemplate(
                "human soldier",
                "You hear a voice coming from your gate. It's a human soldier. He seems tired and injured. Maybe some foo will help him.",
                new List<string> { "Give Foo", "Report to Goblin soliders" },
                new List<EventDelegate> { giveFoo, reportHumanSoldier },
                allNpcPrefabsList[1],1
            ),
            new EventTemplate(
                "Angry Goblin Soldier: Human Soldier",
                "You hear a knock at your gate. It's goblin soldiers. \"Someone saw you helping the human soldiers!\" you betrayed us!",
                new List<string> { "Give up", "fight back" },
                new List<EventDelegate> { GoblinSoldier_GiveUp, GoblinSoldier_FightBack },
                allNpcPrefabsList[4],0
            ),
              new EventTemplate(
                "tax Event",
                "You hear yelling from your gate. You see goblin soldiers standing there. \"We have come today to collect your taxes! This will be crucial to win this war! Now behave and pay your taxes!\"",
                new List<string> { "Pay", "Ignore" },
                new List<EventDelegate> { PayTax, NotPayTax },
                allNpcPrefabsList[3],0
            ),
        };

        eventCurrentDay = 0;

        // Hard code first event to be merchant appearing 2 seconds in.
        nextEventTime = 2f;
        nextEvent = new Event(eventTemplates[0]);

        events = new Queue<Event>();
        AddSpecificEvent("tax Event");
        
    }

    public static void PrintResult(string message)
    {
        // TODO: Make this print to UI.
        Debug.Log(message);
    }

    EventDelegate closeDialog = () =>
    {
        // Do nothing.
        return true;
    };

    EventDelegate giveGrandma = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.RemoveItem("gold", 5);
        Debug.Log(playerInventory.inventory["gold"].GetQuantity());

        playerInventory.RemoveItem("gold", 5);
        PrintResult("You gave 5 gold.");
        philanthropic++;
        return true;
    };

    EventDelegate robGrandma = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 10);
        Debug.Log(playerInventory.inventory["gold"].GetQuantity());
        business--;
        return true;
    };
    EventDelegate giveFoo = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["appleCrop"].GetQuantity() < 2)
        {
            PrintResult("You do not have enough apples.");
            return false;
        }

        playerInventory.RemoveItem("appleCrop", 2);
        Debug.Log(playerInventory.inventory["appleCrop"].GetQuantity());
        UpdateEventPossibility("Angry Goblin Solider: Human Soldier", 1);
        UpdateEventPossibility("human soldier", 0);
        philanthropic++;
        human_loyalty++;
        return true;
    };
    EventDelegate reportHumanSoldier = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 2);
        Debug.Log(playerInventory.inventory["gold"].GetQuantity());
        goblin_loyalty++;
        return true;
    };
    EventDelegate GoblinSoldier_GiveUp = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["appleCrop"].GetQuantity() < 3 ||
            playerInventory.inventory["carrotCrop"].GetQuantity() < 3 ||
            playerInventory.inventory["gold"].GetQuantity() < 5)
        {
            PrintResult("You do not have enough resources to give.");
            return false;
        }

        playerInventory.RemoveItem("appleCrop", 3);
        playerInventory.RemoveItem("carrotCrop", 3);
        playerInventory.RemoveItem("gold", 5);
        PrintResult("Lost 3 apples, 3 carrots, and 5 gold.");
        UpdateEventPossibility("Angry Goblin Soldier: Human Soldier", 0);
        UpdateEventPossibility("human soldier", 1);
        return true;


    };
    EventDelegate GoblinSoldier_FightBack = () => {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ChangeAp(-3);
        PrintResult("Lost 3 AP.");
        UpdateEventPossibility("Angry Goblin Soldier: Human Soldier", 0);
        UpdateEventPossibility("human soldier", 1);
        goblin_loyalty--;
        return true;
    };
    EventDelegate PayTax = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["gold"].GetQuantity() < 5)
        {
            PrintResult("You do not have enough resources to give.");
            return false;
        }

        playerInventory.RemoveItem("gold", 5);
        PrintResult("Lost 5 gold.");
        goblin_loyalty++;
        return true;


    };
    EventDelegate NotPayTax = () => {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ChangeAp(-3);
        PrintResult("Lost 3 AP.");
        goblin_loyalty--;
        return true;


    };

    EventDelegate openShopMenu = () =>
    {
        npc.GetComponent<InventoryUI>().OpenInventory(); //TODO
        return true;
    };

    //Events that occur based on parameters are added through this method
    public bool AddSpecialEvents()
    {
        if (human_loyalty > human_loyalty_guests_threshold && !human_guests_occurred)
        {
            human_guests_occurred = true;
            //AddSpecificEvent("Human Guests");
            //Debug.Log("Special Event added");
            return true;

        }
        else if (human_loyalty > goblin_loyalty_guests_threshold && !goblin_guests_occurred)
        {
            goblin_guests_occurred = true;
            //AddSpecificEvent("Goblin Guests");
           // Debug.Log("Special Event added");

            return true;

        }
        else if (human_loyalty < goblin_loyalty_kick_threshold && !goblin_kick_occured)
        {
            goblin_kick_occured = true;
            //AddSpecificEvent("Goblin Exiled");
            //Debug.Log("Special Event added");

            return true;

        }
        return false;
    }

    //Add a random event to the queue
    public void AddRandomEvent()
    {
        float currentPossibility = UnityEngine.Random.Range(0f, 1f);
        int randomIndex = UnityEngine.Random.Range(0, eventTemplates.Count - 1);
        int testing = 0;
        //Debug.Log("random index:" + randomIndex + "out of "+ (eventTemplates.Count - 1));
        //Debug.Log("last one"+eventTemplates[eventTemplates.Count - 1].name);
    
        //If the event that we are thinking about does not occur based on the possibility, we try to look for another event. This is repeated.
        while (currentPossibility >= eventTemplates[randomIndex].possibility)
        {
            //Debug.Log("random index:" + randomIndex);

            //Debug.Log(randomIndex);
            currentPossibility = UnityEngine.Random.Range(0f, 1f);
            randomIndex = UnityEngine.Random.Range(0, eventTemplates.Count);
            testing++;
            if(testing>=100)
            {
                //Debug.Log("possibily infinite while loop");
                return;
            }
        }
        //Debug.Log(eventTemplates[randomIndex].name + "");

        events.Enqueue(new Event(eventTemplates[randomIndex]));

    }
    //Add an event of name "name"
    public void AddSpecificEvent(string name)
    {
        foreach(EventTemplate template in eventTemplates) 
        {
            if(template.name == name)
            {
                events.Enqueue(new Event(template));
                return;
            }
        }

    }
    public void AddEvent()
    {
        if(AddSpecialEvents())
        {

        }
        else
        {
            AddRandomEvent();
        }

    }

    public float RandomNextEventTime()
    {
        // Earliest time is 5 seconds into the day.
        float earliestTime = (eventCurrentDay * DayTimeController.secondsInADay) + 5f;
        // Latest time is 5 seconds before end of day.
        float latestTime = ((eventCurrentDay + 1) * DayTimeController.secondsInADay) - 5f;

        return UnityEngine.Random.Range(earliestTime, latestTime);
    }

    private static void UpdateEventPossibility(string name, float possibility)
    {
        foreach (EventTemplate template in eventTemplates)
        {
            if(template.name == name) 
            {
                template.possibility = possibility;
                return;
            }
        }
    }

    // Clear listeners on all buttons.
    private void ResetChoiceButtons()
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].onClick.RemoveAllListeners();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("event current day " + eventCurrentDay);
        Debug.Log("next event time " + nextEventTime);
        if (eventCurrentDay <= dayTimeController.getCurrentDay() && nextEvent==null)
        {
            // We hit the next day. Pull out an event from the queue.
            Debug.Log("We started a new day");
            eventCurrentDay = dayTimeController.getCurrentDay()+1;
            if (events.Count > 0)
            {
                nextEventTime = RandomNextEventTime();
                nextEvent = events.Dequeue();
            }
            else
            {
                AddEvent();
            }
        }

        if (nextEvent != null && !nextEvent.eventStarted && nextEventTime < dayTimeController.getCurrentTimeSeconds())
        {
            Debug.Log("length of event: "+ events.Count);
            if (events.Count < 2)
            {
                AddEvent();

            }
            //AddSpecialEvents();
            nextEvent.eventStarted = true;
            npc = Instantiate(nextEvent.template.npcPrefab);
            DialogDelegate dialogDelegate = () =>
            {
                dialogBox.SetActive(true);
                // Clear out all listeners on buttons to make sure we're not accumulating multiple
                // listeners on a single button.
                ResetChoiceButtons();
                dayTimeController.SetPausedTime(true);
                dialogText.text = nextEvent.template.dialogText;
                for (int i = 0; i < choiceButtons.Count; i++)
                {
                    //needed because if you use i directly, i will update between when the listener is set vs when the listener is evaluated
                    int temp = i;
                    Event tempEvent = nextEvent;
                    choiceButtons[i].GetComponentInChildren<TMP_Text>().text = nextEvent.template.choices[i];
                    choiceButtons[i].onClick.AddListener(() =>
                    {
                        bool success = tempEvent.template.executeOption(temp);

                        if (success)
                        {
                            dayTimeController.SetPausedTime(false);
                            dialogBox.SetActive(false);
                            npc.GetComponent<Animator>().SetBool("walkRight", true);
                        }
                        // else keep dialog open and wait for a different choice.
                    });
                }
                nextEvent = null;
            };

            npc.GetComponent<Npc>().SetFields(dialogDelegate);
        }
    }
}

public class Event
{
    public EventTemplate template { get; }
    public bool eventStarted { get; set; }

    public Event(EventTemplate template)
    {
        this.template = template;
        eventStarted = false;
    }
}

public class EventTemplate
{
    public string name;
    public string dialogText;
    public List<string> choices = new List<string>();
    public List<EventDelegate> consequences{ get; set; }
    public GameObject npcPrefab;
    //float ranging from 0-1: When 0, this event will never occur. The higher the possibility, the more likely that this event will occur.
    //When all events are at possibility of 1, they will all occur with same possibility
    //Can be used when events are conditional of other events
    public float possibility;

    public EventTemplate(string name, string dialogText, List<string> choices, List<EventDelegate> consequences, GameObject npcPrefab, float possibility)
    {
        this.name = name;
        this.dialogText = dialogText;
        this.choices = choices;
        this.consequences = consequences;
        this.npcPrefab = npcPrefab;
        this.possibility = possibility;
    }

    public bool executeOption(int option)
    {
        return consequences[option].Invoke();
    }
}

public delegate bool EventDelegate();