using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{

    List<Event> events;
    public DayTimeController dayTimeController;
    public TMP_Text dialogText;
    public List<Button> choiceButtons;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, object> lose5GoldContext = new Dictionary<string, object>()
        {
            { "gained", false},
            { "amount", 5 }
        };
        Dictionary<string, object> gain10GoldContext = new Dictionary<string, object>()
        {
            { "gained", true},
            { "amount", 10 }
        };
        List<EventTemplate> eventTemplates = new List<EventTemplate> {
            new EventTemplate(
                "old lady", 
                "You hear a knock at your gate. \'Would you help me? I'm a poor defenseless grandma and my child is sick. I need 5 gold to buy some medicine\". You ponder your options", 
                new List<string> { "Give gold!", "Rob her!" }, 
                new List<EventDelegate> { changeGold, changeGold },
                new List<Dictionary<string, object>> { lose5GoldContext, gain10GoldContext }
            )
        };

        events = new List<Event>
        {
            new Event(2f, eventTemplates[0])
        };
    }

    EventDelegate changeGold = (Dictionary<string, object> context) => {
        Debug.Log("gold " + ((bool)context["gained"] ? "gained" : "lost") + ": " + context["amount"]);
    };

    // Update is called once per frame
    void Update()
    {
        foreach(Event e in events)
        {
            if(!e.completed && e.time < dayTimeController.getCurrentTimeSeconds())
            {
                dialogText.text = e.template.dialogText;
                for(int i = 0; i < choiceButtons.Count; i++)
                {
                    //needed because if you use i directly, i will update between when the listener is set vs when the listener is evaluated
                    int temp = i;
                    choiceButtons[i].GetComponentInChildren<TMP_Text>().text = e.template.choices[i];
                    choiceButtons[i].onClick.AddListener(()=> { e.template.executeOption(temp); });
                }
                e.completed = true;
            }
        }
    }
}

public class Event
{
    public float time { get; }
    public EventTemplate template { get; }
    public bool completed { get; set; }

    public Event(float time, EventTemplate template)
    {
        this.time = time;
        this.template = template;
        completed = false;
    }
}

public class EventTemplate
{
    public string name;
    public string dialogText;
    public List<string> choices = new List<string>();
    public List<EventDelegate> concequences{ get; set; }
    public List<Dictionary<string, System.Object>> contexts { get; set; }
    public EventTemplate(string name, string dialogText, List<string> choices, List<EventDelegate> concequences, List<Dictionary<string, object>> contexts)
    {
        this.name = name;
        this.dialogText = dialogText;
        this.choices = choices;
        this.concequences = concequences;
        this.contexts = contexts;
    }

    public void executeOption(int option)
    {
        concequences[option].Invoke(contexts[option]);
    }
}

public delegate void EventDelegate(Dictionary<string, System.Object> context);