using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SummaryManager : MonoBehaviour
{
    public Dictionary<SummaryType, object> summary{ get; set; }
    // Start is called before the first frame update
    public void Start()
    {
        summary = new Dictionary<SummaryType, object>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GenerateSummary()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Your stats: ");
        // Dictionary <crop name, count>
        if (summary.ContainsKey(SummaryType.CROP))
        {
            stringBuilder.Append("Crops grown: ");
            bool isFirstDone = false;
            foreach (KeyValuePair<string, int> cropCount in (Dictionary<string, int>)summary[SummaryType.CROP])
            {
                stringBuilder.Append((isFirstDone ? ", " : "") + cropCount.Key + ":" + cropCount.Value.ToString());
                isFirstDone = true;
            }
            stringBuilder.AppendLine();
        }
        // Dictionary <eaten name, count>
        if (summary.ContainsKey(SummaryType.EATEN))
        {
            stringBuilder.Append("Crops eaten: ");
            bool isFirstDone = false;
            foreach (KeyValuePair<string, int> eatenCount in (Dictionary<string, int>)summary[SummaryType.EATEN])
            {
                stringBuilder.Append((isFirstDone ? ", " : "") + eatenCount.Key + ":" + eatenCount.Value.ToString());
                isFirstDone = true;
            }
            stringBuilder.AppendLine();
        }
        // List of tuples. List<List<eventName, eventChoice>>
        if (summary.ContainsKey(SummaryType.EVENT))
        {
            stringBuilder.Append("Events");
            bool isFirstDone = false;
            foreach(List<string> l in (List<List<string>>)summary[SummaryType.EVENT])
            {
                stringBuilder.Append((isFirstDone ? ", " : "") + l[0] + ": " + l[1]);
                isFirstDone = true;
            }
            stringBuilder.AppendLine();
        }
        //inventory
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        Dictionary<string, Item> items = inventory.inventory;
        bool firstItem = true;
        stringBuilder.Append("Inventory: ");
        foreach (Item i in items.Values) {
            if (i.GetQuantity() > 0)
            {
                if (!firstItem)
                {
                    stringBuilder.Append(", ");
                }
                stringBuilder.Append(i.GetItemId().ToString() + ": " + i.GetQuantity().ToString());
                firstItem = false;
            }
        }
        stringBuilder.AppendLine();

        // faction loyalty
        int factionAllegience = EventManager.goblin_loyalty - EventManager.human_loyalty;
        // favor goblins
        if (factionAllegience > 2 )
        {
            stringBuilder.AppendLine("You tended to side with the Goblins");
        } else if (factionAllegience < -2)
        {
            stringBuilder.AppendLine("You tended to side with the Humans");
        } else
        {
            stringBuilder.AppendLine("You stayed relatively neutral in the war");
        }
        return stringBuilder.ToString();
    }

    public enum SummaryType
    {
        CROP,
        EVENT,
        EATEN
    }
}
