using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class EventManager : MonoBehaviour
{
    LinkedList<Event> events;
    Event nextEvent;
    SummaryManager summaryManager;
    // List of tuples <EventName, EventChoice>
    List<List<string>> eventSummary;
    public DayTimeController dayTimeController;
    public GameObject dialogBox;
    public GameObject consequenceBox;
    public TMP_Text consequenceText;
    public List<GameObject> allNpcPrefabsList;
    public GameObject npcManager;
    public GameObject playerPortrait;

    //when the next event that was added to the queue is
    private static float nextEventTime;

    // What day the EventManager thinks it currently is
    private float eventCurrentDay;

    private static GameObject npc;
    private static BarterManager barterManager;

    private static List<EventTemplate> eventTemplates;
    private UIDocument uidoc;
    private List<Button> choiceButtons;
    private GameObject eventTextLabelObj;

    public static int human_loyalty = 0 ;
    public static int goblin_loyalty = 0;

    public static int philanthropic = 0;
    public static int robber_count = 0;
    public static int treasure_count = 0;

    public static int human_loyalty_guests_threshold = 3;
    public static int goblin_loyalty_guests_threshold = 3;
    public static int goblin_loyalty_kick_threshold = -3;

    public bool robber_occurred;
    public bool treasure_owner_occurred;
    public bool angry_goblin_occurred;
    public bool human_guests_occurred;
    public bool goblin_guests_occurred;
    public bool goblin_kick_occurred;
  

// Start is called before the first frame update
public void Start()
    {
        uidoc = dialogBox.GetComponent<UIDocument>();
        choiceButtons = new List<Button>();
        eventTextLabelObj = dialogBox.transform.GetChild(0).gameObject;


        summaryManager = FindAnyObjectByType<SummaryManager>();
        barterManager = GameObject.FindGameObjectWithTag("BarterManager").GetComponent<BarterManager>();
        eventTemplates = new List<EventTemplate> {
            new EventTemplate(
                "merchant",
                "You hear a knock at your gate. \"Would you like to make a trade?\"",
                new List<string> { "Let's Trade", "Not today" },
                new List<EventDelegate> { openShopMenu, EventConsequences.closeDialog },
                allNpcPrefabsList[0],1
            ),
            new EventTemplate(
                "lady", 
                "You hear a knock at your gate. \"Would you help me? I'm a poor defenseless villager and my child is sick. I need 5 gold to buy some medicine\". You ponder your options", 
                new List<string> { "Give gold!", "Rob her!" }, 
                new List<EventDelegate> { EventConsequences.giveGrandma, EventConsequences.robGrandma },
                allNpcPrefabsList[2],1
            ),
            new EventTemplate(
                "human_soldier",
                "You hear a voice coming from your gate. It's a human soldier. He seems tired and injured. Maybe some foo will help him.",
                new List<string> { "Give Foo", "Report to Goblin soliders" },
                new List<EventDelegate> { EventConsequences.giveFoo, EventConsequences.reportHumanSoldier },
                allNpcPrefabsList[1],1
            ),
            new EventTemplate(
                "angry_goblin",
                "You hear a knock at your gate. It's goblin soldiers. \"Someone saw you helping the human soldiers!\" you betrayed us!",
                new List<string> { "Give up", "fight back" },
                new List<EventDelegate> { EventConsequences.GoblinSoldier_GiveUp, EventConsequences.GoblinSoldier_FightBack },
                allNpcPrefabsList[4],0
            ),
            new EventTemplate(
                "tax_goblin",
                "You hear yelling from your gate. You see goblin soldiers standing there. \"We have come today to collect your taxes! This will be crucial to win this war! Now behave and pay your taxes!\"",
                new List<string> { "Pay", "Ignore" },
                new List<EventDelegate> { EventConsequences.PayTax, EventConsequences.NotPayTax },
                allNpcPrefabsList[3],1
            ),
            new EventTemplate(
                "robber",
                "A disheveled goblin crashes through your gate. \"Oye! I heard you've been robbing passerbys in these parts. There's only enough room for one robber here!\"",
                new List<string> { "Hand over money", "Fight" },
                new List<EventDelegate> { EventConsequences.PayRobber, EventConsequences.FightRobber },
                allNpcPrefabsList[4],0
            ),
            new EventTemplate(
                "treasure",
                "A treasure chest walks up to your front door...",
                new List<string> { "Open Chest", "Ignore" },
                new List<EventDelegate> { EventConsequences.OpenChest, EventConsequences.closeDialog },
                allNpcPrefabsList[8],1
            ),
            new EventTemplate(
                "treasure_owner",
                "A man walks up to your door. \"My treasure chest grew legs and ran off! Have you seen it?\"",
                new List<string> { "Return money", "Say no" },
                new List<EventDelegate> { EventConsequences.TreasureOwnerReturnMoney, EventConsequences.TreasureOwnerSayNo },
                allNpcPrefabsList[5],0
            ),
            new EventTemplate(
                "treasure_mimic",
                "A treasure chest walks up to your front door...",
                new List<string> { "Open Chest", "Ignore" },
                new List<EventDelegate> { EventConsequences.OpenMimicChest, EventConsequences.closeDialog },
                allNpcPrefabsList[8],1
            ),
            new EventTemplate(
                "rain",
                "A cloud covers the sun for a brief moment and you feel rain against your forehead.",
                new List<string> { "I'm drenched", "I'm still drenched" },
                new List<EventDelegate> { EventConsequences.Rain, EventConsequences.Rain },
                null,1
            ),
        };

        eventCurrentDay = 0;

        // Hard code first event to be merchant appearing 10 seconds in.
        nextEventTime = 10f;
        nextEvent = new Event(eventTemplates[0]);

        events = new LinkedList<Event>();
        AddSpecificEvent("tax_goblin", true);
        AddSpecificEvent("merchant", true);
    }

    public void PrintResultAfterDelay(float delay, string message)
    {
        StartCoroutine(WaitAndPrintResult(delay, message, 2f));
    }

    public void PrintResultAfterDelay(float delay, string message, float visibleTime)
    {
        StartCoroutine(WaitAndPrintResult(delay, message, visibleTime));
    }

    IEnumerator WaitAndPrintResult(float delay, string message, float visibleTime)
    {
        yield return new WaitForSeconds(delay);
        PrintResult(message, visibleTime);
    }

    public void PrintResult(string message)
    {
        PrintResult(message, 2f);
    }

    public void PrintResult(string message, float visibleTime)
    {
        consequenceBox.SetActive(true);
        consequenceText.text = message;

        StartCoroutine(WaitAndDisableConsequence(visibleTime));
    }

    IEnumerator WaitAndDisableConsequence(float visibleTime)
    {
        yield return new WaitForSeconds(visibleTime);
        consequenceBox.SetActive(false);
    }

    EventDelegate openShopMenu = () =>
    {
        barterManager.startTrading(npc.gameObject);
        return true;
    };

    //Events that occur based on parameters are added through this method
    // Override whatever the next random event was going to be.
    public bool AddSpecialEvents()
    {
        if (robber_count >= 2 && !robber_occurred)
        {
            robber_occurred = true;
            ReplaceNextEvent("robber");
            return true;
        }
        if (human_loyalty >= 1 && !angry_goblin_occurred)
        {
            angry_goblin_occurred = true;
            ReplaceNextEvent("angry_goblin");
            return true;
        }
        if (treasure_count >= 1 && !treasure_owner_occurred)
        {
            treasure_owner_occurred = true;
            ReplaceNextEvent("treasure_owner");
            return true;
        }
        //if (human_loyalty > human_loyalty_guests_threshold && !human_guests_occurred)
        //{
        //    human_guests_occurred = true;
        //    return true;
        //}
        //if (goblin_loyalty > goblin_loyalty_guests_threshold && !goblin_guests_occurred)
        //{
        //    goblin_guests_occurred = true;
        //    return true;
        //}
        //if (human_loyalty < goblin_loyalty_kick_threshold && !goblin_kick_occurred)
        //{
        //    goblin_kick_occurred = true;
        //    return true;
        //}
        return false;
    }

    //Add a random event to the queue
    public void AddRandomEvent()
    {
        float currentPossibility = UnityEngine.Random.Range(0f, 1f);
        int randomIndex = UnityEngine.Random.Range(0, eventTemplates.Count - 1);
        int testing = 0;
    
        //If the event that we are thinking about does not occur based on the possibility, we try to look for another event. This is repeated.
        while (currentPossibility >= eventTemplates[randomIndex].possibility)
        {
            currentPossibility = UnityEngine.Random.Range(0f, 1f);
            randomIndex = UnityEngine.Random.Range(0, eventTemplates.Count);
            testing++;
            if(testing>=100)
            {
                return;
            }
        }

        events.AddLast(new Event(eventTemplates[randomIndex]));

    }
    //Add an event of name "name"
    public void AddSpecificEvent(string name, bool addLast)
    {
        foreach(EventTemplate template in eventTemplates) 
        {
            if(template.name == name)
            {
                if (addLast)
                {
                    events.AddLast(new Event(template));
                }
                else
                {
                    events.AddFirst(new Event(template));
                }
                return;
            }
        }
    }
    public void ReplaceNextEvent(string name)
    {
        foreach (EventTemplate template in eventTemplates)
        {
            if (template.name == name)
            {
                nextEvent = new Event(template);
            }
        }
    }

    public void AddEvent()
    {
        bool success = AddSpecialEvents();
        if(!success)
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
        choiceButtons.Clear();
        //for (int i = 0; i < choiceButtons.Count; i++)
        //{
        //    choiceButtons[i].onClick.RemoveAllListeners();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (eventCurrentDay < dayTimeController.getCurrentDay() && nextEvent==null)
        {
            // We hit the next day. Pull out an event from the queue.
            eventCurrentDay = dayTimeController.getCurrentDay();
            if (events.Count > 0)
            {
                nextEventTime = RandomNextEventTime();
                nextEvent = events.First.Value;
                events.RemoveFirst();
            }
            AddEvent();
        }

        if (nextEvent != null && !nextEvent.eventStarted && nextEventTime < dayTimeController.getCurrentTimeSeconds())
        {
            nextEvent.eventStarted = true;
            if(nextEvent.template.npcPrefab == null)
            {
                dialogBox.SetActive(true);
                // Clear out all listeners on buttons to make sure we're not accumulating multiple
                // listeners on a single button.
                ResetChoiceButtons();
                dayTimeController.SetPausedTime(true);
                VisualElement root = uidoc.rootVisualElement;
                root.Q<Label>("Label").text = nextEvent.template.dialogText;
                choiceButtons.Add(root.Q<Button>("acceptButton"));
                choiceButtons.Add(root.Q<Button>("declineButton"));
                for (int i = 0; i < choiceButtons.Count; i++)
                {
                    //needed because if you use i directly, i will update between when the listener is set vs when the listener is evaluated
                    int temp = i;
                    Event tempEvent = nextEvent;
                    string tempChoice = nextEvent.template.choices[i];
                    choiceButtons[i].clicked += () =>
                    {
                        bool success = tempEvent.template.executeOption(temp);

                        if (success)
                        {
                            InitializeEventSummaryIfNotExist();
                            eventSummary.Add(new List<string> { tempEvent.template.name, tempChoice });
                            dayTimeController.SetPausedTime(false);
                            dialogBox.SetActive(false);
                            //playerPortrait.SetActive(false);
                            //npc.transform.GetChild(3).gameObject.SetActive(false);
                            //npc.GetComponent<Animator>().SetBool("walkRight", true);
                        }
                        // else keep dialog open and wait for a different choice.
                    };
                }
                nextEvent = null;

            } else
            {
                
                npc = Instantiate(nextEvent.template.npcPrefab);
                DialogDelegate dialogDelegate = () =>
                {
                    dialogBox.SetActive(true);
                    npc.transform.GetChild(3).gameObject.SetActive(true);
                    playerPortrait.SetActive(true);
                    // Clear out all listeners on buttons to make sure we're not accumulating multiple
                    // listeners on a single button.
                    ResetChoiceButtons();
                    dayTimeController.SetPausedTime(true);
                    VisualElement root = uidoc.rootVisualElement;
                    Label dialogDisplay = eventTextLabelObj.GetComponent<UIDocument>().rootVisualElement.Q<Label>("label");
                    dialogDisplay.text = nextEvent.template.dialogText;
                    choiceButtons.Add(root.Q<Button>("acceptButton"));
                    choiceButtons.Add(root.Q<Button>("declineButton"));
                    for (int i = 0; i < choiceButtons.Count; i++)
                    {
                        //needed because if you use i directly, i will update between when the listener is set vs when the listener is evaluated
                        int temp = i;
                        Event tempEvent = nextEvent; 
                        string tempChoice = nextEvent.template.choices[i];
                        choiceButtons[i].text = tempChoice;
                        choiceButtons[i].clicked += () =>
                        {
                            bool success = tempEvent.template.executeOption(temp);

                            if (success)
                            {
                                InitializeEventSummaryIfNotExist();
                                eventSummary.Add(new List<string> { tempEvent.template.name, tempChoice });
                                dayTimeController.SetPausedTime(false);
                                dialogBox.SetActive(false);
                                //playerPortrait.SetActive(false);
                                //npc.transform.GetChild(3).gameObject.SetActive(false);
                                //npc.GetComponent<Animator>().SetBool("walkRight", true);
                            }
                            // else keep dialog open and wait for a different choice.
                        };
                    }
                    nextEvent = null;
                };

                npc.GetComponent<Npc>().SetFields(dialogDelegate);
            }

        }
    }
    private void InitializeEventSummaryIfNotExist()
    {
        //Set up summary
        Dictionary<SummaryManager.SummaryType, object> summary = summaryManager.summary;
        if (!summary.ContainsKey(SummaryManager.SummaryType.EVENT))
        {
            summary.Add(SummaryManager.SummaryType.EVENT, new List<List<string>>());
            eventSummary = (List<List<string>>)summary[SummaryManager.SummaryType.EVENT];
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