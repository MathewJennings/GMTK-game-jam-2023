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

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Gray out options that you are unable to do.
        List<EventTemplate> eventTemplates = new List<EventTemplate> {
            new EventTemplate(
                "merchant",
                "You hear a knock at your gate. \"Would you like to make a trade?",
                new List<string> { "Browse", "Ignore" },
                new List<EventDelegate> { openShopMenu, closeDialog },
                allNpcPrefabsList[0]
            ),
            new EventTemplate(
                "old lady", 
                "You hear a knock at your gate. \"Would you help me? I'm a poor defenseless grandma and my child is sick. I need 5 gold to buy some medicine\". You ponder your options", 
                new List<string> { "Give gold!", "Rob her!" }, 
                new List<EventDelegate> { giveGrandma, robGrandma },
                allNpcPrefabsList[0]
            ),
        };

        // Next event needs to have timestamp less than everything in events, and all events in
        // the queue must be in order.
        nextEvent = new Event(2f, eventTemplates[1]);
        events = new Queue<Event>();
        events.Enqueue(new Event(4f, eventTemplates[1]));
        events.Enqueue(new Event(6f, eventTemplates[1]));
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

    EventDelegate openShopMenu = () =>
    {
        // Open shop UI.
    };

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
        if(nextEvent != null && !nextEvent.eventStarted && nextEvent.time < dayTimeController.getCurrentTimeSeconds())
        {
            nextEvent.eventStarted = true;
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
                    });
                }
                nextEvent = events.Count > 0 ? events.Dequeue() : null;
            };

            GameObject npc = Instantiate(nextEvent.template.npcPrefab);
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
    public EventTemplate(string name, string dialogText, List<string> choices, List<EventDelegate> consequences, GameObject npcPrefab)
    {
        this.name = name;
        this.dialogText = dialogText;
        this.choices = choices;
        this.consequences = consequences;
        this.npcPrefab = npcPrefab;
    }

    public void executeOption(int option)
    {
        consequences[option].Invoke();
    }
}

public delegate void EventDelegate();