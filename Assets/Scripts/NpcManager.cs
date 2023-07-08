using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    DayTimeController dayTimeController;
    public List<Npc> allNpcs;
    // Start is called before the first frame update
    void Start()
    {
        dayTimeController = FindAnyObjectByType<DayTimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNpc(NpcType npcType)
    {
        switch (npcType)
        {
            case NpcType.MERCHANT:
                InitializeNpc(0);
                break;
        }
    }

    void InitializeNpc(int index)
    {

    }
    public enum NpcType
    {
        MERCHANT,
        ROBBER
    }
}
