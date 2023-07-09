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
            foreach(KeyValuePair<string, int> cropCount in (Dictionary<string, int>)summary[SummaryType.CROP])
            {
                stringBuilder.AppendLine(cropCount.Key + ":" + cropCount.Value.ToString() + "\n");
            }
        }
        // Dictionary <eaten name, count>
        if (summary.ContainsKey(SummaryType.EATEN))
        {
            foreach (KeyValuePair<string, int> eatenCount in (Dictionary<string, int>)summary[SummaryType.EATEN])
            {
                stringBuilder.AppendLine(eatenCount.Key + ":" + eatenCount.Value.ToString() + "\n");
            }
        }
        // List of tuples. List<List<eventName, eventChoice>>
        if (summary.ContainsKey(SummaryType.EVENT))
        {
            foreach(List<string> l in (List<List<string>>)summary[SummaryType.EVENT])
            {
                stringBuilder.AppendLine(l[0] + ": " + l[1]);
            }
        }
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
