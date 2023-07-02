using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//there will eventually be subclasses, BigBrother and LittleBrother
public class Player : MonoBehaviour
{
    public int startingHp;
    // there will eventually be an object for perks
    public GameObject perks;
    //there will eventually be an object for debuffs
    public GameObject debuffs;
    //state will be "carrying item", "default", "being carried", "unconscious"
    public GameObject state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
