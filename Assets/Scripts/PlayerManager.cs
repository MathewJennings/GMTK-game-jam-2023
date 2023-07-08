using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int maxAp;
    public int maxHunger;

    int ap;
    int hunger;

    // Start is called before the first frame update
    void Start()
    {
        ap = maxAp; 
        hunger = maxHunger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("ReduceApBy5")]
    public void testReduceApBy5()
    {
        ChangeAp(-5);
    }

    public void ChangeAp(int delta)
    {
        ap += delta;
        if(ap <= 0)
        {
            Debug.Log("You are overworked. You died");
        }
    }

    [ContextMenu("Reduce hunger by 5")]
    public void testReduceHungerBy5()
    {
        ChangeHunger(-5);
    }

    public void ChangeHunger(int delta)
    {
        hunger += delta;
        if(hunger <= 0)
        {
            Debug.Log("You died as you lived, hungry and alone.");
        }
    }
}
