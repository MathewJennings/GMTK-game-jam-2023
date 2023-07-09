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
    public GameObject eventOverlay;
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
    private UIDocument eventButtonsDoc;

    public static int human_loyalty = 0 ;
    public static int goblin_loyalty = 0;

    public static int philanthropic = 0;
    public static int refugee_denied_count = 0;
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

    public const float tutorialMessageTime = 5f;
  

// Start is called before the first frame update
public void Start()
    {
        uidoc = dialogBox.GetComponent<UIDocument>();
        choiceButtons = new List<Button>();
        eventTextLabelObj = dialogBox.transform.GetChild(0).gameObject;
        eventButtonsDoc = dialogBox.transform.GetChild(1).gameObject.GetComponent<UIDocument>();


        summaryManager = FindAnyObjectByType<SummaryManager>();
        barterManager = GameObject.FindGameObjectWithTag("BarterManager").GetComponent<BarterManager>();
        eventTemplates = new List<EventTemplate> {
            new EventTemplate(
                "merchant",
                "A stranger walks up to your gate, a trader by the looks of her. You've been anxious of visitors ever since the human victory in the war started to become a slow inevitability. She approaches cautiously and, gauging you to be non-violent, asks you if you'd like to trade.",
                new List<string> { "Take a look at her wares", "Ask her to be on her way" },
                new List<EventDelegate> { openShopMenu, EventConsequences.closeDialog },
                allNpcPrefabsList[0],1
            ),
            new EventTemplate(
                "lady", 
                "A human has discovered your farm, a refugee by the looks of her. Clearly desperate, she asks for your help. \"My child and I have been fleeing the war, but the wandering mercenaries are even worse than the soldiers. Please, my child is sick. Can you spare any gold to help?\". You ponder your options...", 
                new List<string> { "Take pity on her (5G)", "Shoo her away" }, 
                new List<EventDelegate> { EventConsequences.giveGrandma, EventConsequences.sendGrandmaAway },
                allNpcPrefabsList[2],1
            ),
            new EventTemplate(
                "human_soldier",
                "A limping human soldier discovered your farm and approached, seemingly in desperation. You quickly realize he's no threat, and as he gets closer you can see his left leg was crudely amputated. It smells infected. He begs for you to spare some food with him, any food at all.",
                new List<string> { "Share some of your food (2)", "Report him to the defense army later" },
                new List<EventDelegate> { EventConsequences.giveFood, EventConsequences.reportHumanSoldier },
                allNpcPrefabsList[1],1
            ),
            new EventTemplate(
                "angry_goblin",
                "Goblin soldiers appraoch your farm. They look violent and angry. They scream that they know you helped a human solider and that you betrayed the resistance. Spittle drops from their chins as they gesticulate wildly. They tell you they'd kill you right now if not for your food production.",
                new List<string> { "Give them everything you have to placate them", "Brace yourself for their violence" },
                new List<EventDelegate> { EventConsequences.GoblinSoldier_OfferInventory, EventConsequences.GoblinSoldier_Assault },
                allNpcPrefabsList[4],0
            ),
            new EventTemplate(
                "tax_goblin",
                "You hear roudy voices approaching your farm. A small group of mercenaries has arrived. These bands have become more and more common of late. This one looks particularly familiar with violence -- nasty even. \"What do you want?\" you ask them. \"Taxes! You're gonna help us in the fight against the humans, aren't you?\" You've been extorted before, and you'll be extorted again.",
                new List<string> { "Give them what they want (5G)", "Don't and deal with the consequences" },
                new List<EventDelegate> { EventConsequences.PayTax, EventConsequences.NotPayTax },
                allNpcPrefabsList[3],1
            ),
            new EventTemplate(
                "robber",
                "Very suddenly, a disheveled goblin crashes onto your farm. He's hyped up on fisstech, by the look of it. He seems aware of the stream of refugees passing through the area.  \"Oye! I bet you've had your way with a bunch of passerbys in these parts. An enterprising fellow like yourself has probably gotten fat and happy from it all huh?\" His threat is clear.",
                new List<string> { "Give him what he wants", "Refuse and defend yourself" },
                new List<EventDelegate> { EventConsequences.PayRobber, EventConsequences.AttackedByRobber },
                allNpcPrefabsList[4],0
            ),
            new EventTemplate(
                "treasure",
                "You can't believe it. A treasure chest has somehow walked up to your farm. You reason that magicians must be fighting in the war now -- an indication that its reaching its climax. Regardless, you figure this is probably a blessing of pure luck. You consider your options...",
                new List<string> { "Claim the chest", "Too risky..." },
                new List<EventDelegate> { EventConsequences.OpenChest, EventConsequences.IgnoreChest },
                allNpcPrefabsList[8],1
            ),
            new EventTemplate(
                "treasure_owner",
                "A mage approaches your farm. You notice that he is brimming with magical energy as the atmosphere in his immediate vicinity electrifies. This is a dangerous man. Aware of the power imbalance, he confidently asks you if you've seen his magic chest. He is clearly expecting you to say yes.",
                new List<string> { "Fess up and pay him back", "Grovel and feign ignorance" },
                new List<EventDelegate> { EventConsequences.TreasureOwnerReturnMoney, EventConsequences.TreasureOwnerSayNo },
                allNpcPrefabsList[5],0
            ),
            new EventTemplate(
                "treasure_mimic",
                "You can't believe it. A treasure chest has somehow walked up to your farm. You reason that magicians must be fighting in the war now -- an indication that its reaching its climax. Regardless, you figure this is probably a blessing of pure luck. You consider your options...",
                new List<string> { "Claim the chest", "Too risky..." },
                new List<EventDelegate> { EventConsequences.OpenMimicChest, EventConsequences.IgnoreChest },
                allNpcPrefabsList[8],1
            ),
            new EventTemplate(
                "rain",
                "Clouds move in, blanketing the sky. It's a preciously rare sight these days. Before you know it, you start to feel water on your forehead. It rapidly swells to a dark torrent. As you stand in the down pour you feel a well of appreciation for today's respite.",
                new List<string> { "Rest", "Reflect" },
                new List<EventDelegate> { EventConsequences.Rain, EventConsequences.Rain },
                null,1
            ),
            new EventTemplate(
                "drought",
                "The sun's heat intensifies today. An oppressive miasma of humidity suffocates you as you watch your plants start to shrivel in the heat.",
                new List<string> { "Time to get to work", "No rest for the wicked..." },
                new List<EventDelegate> { EventConsequences.Drought, EventConsequences.Drought },
                null,1
            ),
            new EventTemplate(
                "explosions",
                "You suddenly hear loud explosions nearby. It must be a battle nearby, including mages. You sense imminent danger and pray to your gods that you and your land aren't caught in the crossfire...",
                new List<string> { "Brace", "Seek shelter" },
                new List<EventDelegate> { EventConsequences.Explosion, EventConsequences.Explosion },
                null,1
            ),
            new EventTemplate(
                "wealthy_merchant",
                "The air smells like gold and arrogance. You hear an anticipated knock on your door. The wealthy merchant has appeared at your door.",
                new List<string> { "Take a look at her wares?", "Ask her to leave." },
                new List<EventDelegate> { openShopMenu, EventConsequences.closeDialog },
                allNpcPrefabsList[9],1
            ),
            new EventTemplate(
                "final_event_human",
                "An important human emissary has approached your farm. He says that the final battle is drawing near and that the humans need your support. He's asking you to lend a cornucopia of money and food to help them land the decisive blow. They want 10 carrot crops, 10 apple crops, and 15 gold.",
                new List<string> { "Offer your support to the humans", "Apologize and deny the request" },
                new List<EventDelegate> { EventConsequences.SupportHumanVictory, EventConsequences.RejectHumanVictory },
                allNpcPrefabsList[5],0
            ),
            new EventTemplate(
                "final_event_goblin",
                "An important goblin emissary has approached your farm. He says that the final battle is drawing near and that the goblins need your support to fend off the human army. He's asking you to lend your support via gold and a cornucopia of food to help them land the decisive blow. They want 10 carrot crops, 10 apple crops, and 15 gold.",
                new List<string> { "Offer your support to the goblins", "Apologize and deny the request" },
                new List<EventDelegate> { EventConsequences.SupportGoblinVictory, EventConsequences.RejectGoblinVictory},
                allNpcPrefabsList[7],0
            )
        };

        eventCurrentDay = 0;

        // Hard code first event to be merchant appearing 10 seconds in.
        nextEventTime = 10f;
        nextEvent = GetSpecificEvent("merchant");

        events = new LinkedList<Event>();
        AddRandomEvent();
        AddSpecificEvent("tax_goblin", true);
        AddSpecificEvent("wealthy_merchant", true);
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
        if (refugee_denied_count >= 2 && !robber_occurred)
        {
            robber_occurred = true;
            nextEvent = GetSpecificEvent("robber");
            return true;
        }
        if (human_loyalty >= 1 && !angry_goblin_occurred)
        {
            angry_goblin_occurred = true;
            nextEvent = GetSpecificEvent("angry_goblin");
            return true;
        }
        if (treasure_count >= 1 && !treasure_owner_occurred)
        {
            treasure_owner_occurred = true;
            nextEvent = GetSpecificEvent("treasure_owner");
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
        Event e = GetSpecificEvent(name);
        if (e == null)
        {
            return;
        }

        if (addLast)
        {
            events.AddLast(e);
        }
        else
        {
            events.AddFirst(e);
        }
    }

    public Event GetSpecificEvent(string name)
    {
        foreach (EventTemplate template in eventTemplates)
        {
            if (template.name == name)
            {
                return new Event(template);
            }
        }
        return null;
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

            // If it's the final day before winning, hard code the final event.
            if (eventCurrentDay >= dayTimeController.numDaysToWin - 1)
            {
                nextEventTime = RandomNextEventTime();
                if (human_loyalty > goblin_loyalty)
                {
                    nextEvent = GetSpecificEvent("final_event_human");
                }
                else
                {
                    nextEvent = GetSpecificEvent("final_event_goblin");
                }
            }
            else
            {
                if (events.Count > 0)
                {
                    nextEventTime = RandomNextEventTime();
                    nextEvent = events.First.Value;
                    events.RemoveFirst();
                }
                AddEvent();
            }
        }

        if (nextEvent != null && !nextEvent.eventStarted && nextEventTime < dayTimeController.getCurrentTimeSeconds())
        {
            nextEvent.eventStarted = true;
            if(nextEvent.template.npcPrefab == null)
            {
                dialogBox.SetActive(true);
                eventOverlay.SetActive(true);
                // Clear out all listeners on buttons to make sure we're not accumulating multiple
                // listeners on a single button.
                ResetChoiceButtons();
                dayTimeController.SetPausedTime(true);
                VisualElement root = uidoc.rootVisualElement;
                root.Q<Label>("label").text = nextEvent.template.dialogText;
                VisualElement eventButtonsRoot = eventButtonsDoc.rootVisualElement;
                choiceButtons.Add(eventButtonsRoot.Q<Button>("acceptButton"));
                choiceButtons.Add(eventButtonsRoot.Q<Button>("declineButton"));
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
                            eventOverlay.SetActive(false);
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
                    eventOverlay.SetActive(true);
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
                                List<string> events = new List<string>();
                                foreach (List<string> singleEventSummary in eventSummary)
                                {
                                    events.Add(singleEventSummary[0]);
                                }
                                if(events.Contains("final_event_goblin") || events.Contains("final_event_human"))
                                {
                                    dayTimeController.SetPausedTime(true);
                                }
                                else
                                {
                                    dayTimeController.SetPausedTime(false);
                                }
                                dialogBox.SetActive(false);
                                eventOverlay.SetActive(false);
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