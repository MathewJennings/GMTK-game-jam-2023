using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    //when the last event that was added to the queue is
    private float lastEventTime;

    //when the next event that was added to the queue is
    private float nextEventTime;

    //minimum time that needs to pass since the last event for the next event to occur
    private float minEventGap = 5f;

    //maximum time that needs to pass since the last event for the next event to occur
    private float maxEventGap = 10f;

    private static GameObject npc;


    private static List<EventTemplate> eventTemplates;



    // Start is called before the first frame update
    void Start()
    {
        // TODO: Gray out options that you are unable to do.
        eventTemplates = new List<EventTemplate> {
            new EventTemplate(
                "merchant",
                "You hear a knock at your gate. \"Would you like to make a trade?",
                new List<string> { "Let's Trade", "Ignore" },
                new List<EventDelegate> { openShopMenu, closeDialog },
                allNpcPrefabsList[0],1,0
            ),
            new EventTemplate(
                "old lady", 
                "You hear a knock at your gate. \"Would you help me? I'm a poor defenseless grandma and my child is sick. I need 5 gold to buy some medicine\". You ponder your options", 
                new List<string> { "Give gold!", "Rob her!" }, 
                new List<EventDelegate> { giveGrandma, robGrandma },
                allNpcPrefabsList[0],1,0
            ),
            new EventTemplate(
                "human soldier",
                "You hear a voice coming from your gate. It's a human soldier. He seems tired and injured. Maybe some foo will help him.",
                new List<string> { "Give Foo", "Report to Goblin soliders" },
                new List<EventDelegate> { giveFoo, reportHumanSoldier },
                allNpcPrefabsList[0],1,0
            ),
            new EventTemplate(
                "Angry Goblin Solider: Human Soldier",
                "You hear a knock at your gate. It's goblin soldiers. \"Someone saw you helping the human soldiers!\" you betrayed us!",
                new List<string> { "Give up", "fight back" },
                new List<EventDelegate> { GoblinSoldier_GiveUp, GoblinSoldier_FightBack },
                allNpcPrefabsList[0],0,0
            ),
        };

        // Next event needs to have timestamp less than everything in events, and all events in
        // the queue must be in order.
        nextEventTime = UnityEngine.Random.Range(minEventGap, maxEventGap);
        nextEvent = new Event(nextEventTime, eventTemplates[0]);
        events = new Queue<Event>();
        lastEventTime = nextEventTime;
        AddRandomEvent();
        AddRandomEvent();
        AddRandomEvent();






    }

    EventDelegate closeDialog = () =>
    {
        // Do nothing.
    };

    EventDelegate giveGrandma = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.RemoveItem("gold", 5);
        Debug.Log(playerInventory.inventory["gold"].GetQuantity());

    };

    EventDelegate robGrandma = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 10);
        Debug.Log(playerInventory.inventory["gold"].GetQuantity());
    };
    EventDelegate giveFoo = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.RemoveItem("appleCrop", 2);
        Debug.Log(playerInventory.inventory["appleCrop"].GetQuantity());
        UpdateEventPossibility("Angry Goblin Solider: Human Soldier", 1);
        UpdateEventPossibility("human soldier", 0);
    };
    EventDelegate reportHumanSoldier = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 2);
        Debug.Log(playerInventory.inventory["gold"].GetQuantity());
    };
    EventDelegate GoblinSoldier_GiveUp = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.RemoveItem("appleCrop", 3);
        playerInventory.RemoveItem("carrotCrop", 3);
        playerInventory.RemoveItem("gold", 5);
        UpdateEventPossibility("Angry Goblin Solider: Human Soldier", 0);
        UpdateEventPossibility("human soldier", 1);

    };
    EventDelegate GoblinSoldier_FightBack = () => {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ChangeAp(-3);
        UpdateEventPossibility("Angry Goblin Solider: Human Soldier", 0);
        UpdateEventPossibility("human soldier", 1);

    };

    EventDelegate openShopMenu = () =>
    {
        npc.GetComponent<InventoryUI>().OpenInventory(); //TODO
    };


    //Add a random event to the queue
    public void AddRandomEvent()
    {
       nextEventTime = lastEventTime + UnityEngine.Random.Range(minEventGap, maxEventGap);
        float currentPossibility = UnityEngine.Random.Range(0f, 1f);
       int randomIndex = UnityEngine.Random.Range(0, eventTemplates.Count);
        int testing = 0;
        Debug.Log("random index:" + randomIndex + "out of "+ (eventTemplates.Count - 1));
        Debug.Log("last one"+eventTemplates[eventTemplates.Count - 1].name);
    
        //If the event that we are thinking about does not occur based on the possibility, we try to look for another event. This is repeated.
        while (currentPossibility >= eventTemplates[randomIndex].possibility || eventTemplates[randomIndex].minTime>dayTimeController.getCurrentTimeSeconds())
        {
            Debug.Log("random index:" + randomIndex);

            Debug.Log(randomIndex);
            currentPossibility = UnityEngine.Random.Range(0f, 1f);
            randomIndex = UnityEngine.Random.Range(0, eventTemplates.Count);
            testing++;
            if(testing>=100)
            {
                Debug.Log("possibily infinite while loop");
                return;
            }
        }
        Debug.Log(eventTemplates[randomIndex].name + "");

        events.Enqueue(new Event(nextEventTime, eventTemplates[randomIndex]));
        lastEventTime = nextEventTime;

    }
    //Add an event of name "name"
    public void AddEvent(string name)
    {
        nextEventTime = lastEventTime + UnityEngine.Random.Range(minEventGap, maxEventGap);
        foreach(EventTemplate template in  eventTemplates) 
        {
            if(template.name == name)
            {
                events.Enqueue(new Event(nextEventTime, template));
                lastEventTime = nextEventTime;
                return;
            }
        }

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
        //if(events.Count<2)
        //{
        //    AddRandomEvent();
        //}
        if (nextEvent != null && !nextEvent.eventStarted && nextEvent.time < dayTimeController.getCurrentTimeSeconds())
        {
            Debug.Log("length of event: "+ events.Count);
            if (events.Count < 2)
            {
                AddRandomEvent();
                Debug.Log("adding an event");

            }
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
                        tempEvent.template.executeOption(temp);
                        dayTimeController.SetPausedTime(false);
                        dialogBox.SetActive(false);
                        npc.GetComponent<Animator>().SetBool("walkRight", true);
                    });
                }
                nextEvent = events.Count > 0 ? events.Dequeue() : null;
            };

            npc.GetComponent<Npc>().SetFields(dialogDelegate);
        }
    }
}

public class Event
{
    public float time { get; }
    public EventTemplate template { get; }
    public bool eventStarted { get; set; }

    public Event(float time, EventTemplate template)
    {
        this.time = time;
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

    //Earliest when this event can occur.
    public float minTime;

    public EventTemplate(string name, string dialogText, List<string> choices, List<EventDelegate> consequences, GameObject npcPrefab, float possibility, float minTime)
    {
        this.name = name;
        this.dialogText = dialogText;
        this.choices = choices;
        this.consequences = consequences;
        this.npcPrefab = npcPrefab;
        this.possibility = possibility;
        this.minTime = minTime;
    }

    public void executeOption(int option)
    {
        consequences[option].Invoke();
    }
}

public delegate void EventDelegate();