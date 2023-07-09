using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsequences
{
    public static EventDelegate closeDialog = () =>
    {
        // Do nothing.
        return true;
    };

    public static EventDelegate giveGrandma = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["gold"].GetQuantity() < 5)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have enough gold.");
            return false;
        }

        playerInventory.RemoveItem("gold", 5);
        EventManager.philanthropic++;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You gave 5 gold.");
        return true;
    };

    public static EventDelegate robGrandma = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 10);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You got 5 gold.");
        EventManager.robber_count++;
        return true;
    };
    public static EventDelegate giveFoo = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["appleCrop"].GetQuantity() < 2)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have enough apples.");
            return false;
        }

        playerInventory.RemoveItem("appleCrop", 2);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You gave 2 apples.");
        EventManager.philanthropic++;
        EventManager.human_loyalty++;
        return true;
    };
    public static EventDelegate reportHumanSoldier = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        EventManager.goblin_loyalty++;
        playerInventory.AddItem("gold", 2);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You got 2 gold.");
        return true;
    };
    public static EventDelegate GoblinSoldier_GiveUp = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["appleCrop"].GetQuantity() < 3 ||
            playerInventory.inventory["carrotCrop"].GetQuantity() < 3 ||
            playerInventory.inventory["gold"].GetQuantity() < 5)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have enough resources to give.");
            return false;
        }

        playerInventory.RemoveItem("appleCrop", 3);
        playerInventory.RemoveItem("carrotCrop", 3);
        playerInventory.RemoveItem("gold", 5);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Lost 3 apples, 3 carrots, and 5 gold.");
        return true;


    };
    public static EventDelegate GoblinSoldier_FightBack = () => {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ChangeAp(-3);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Lost 3 AP.");
        EventManager.goblin_loyalty--;
        return true;
    };
    public static EventDelegate PayTax = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["gold"].GetQuantity() < 5)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have enough resources to give.");
            return false;
        }

        playerInventory.RemoveItem("gold", 5);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Lost 5 gold.");
        EventManager.goblin_loyalty++;
        return true;
    };
    public static EventDelegate NotPayTax = () => {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ChangeAp(-3);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Lost 3 AP.");
        EventManager.goblin_loyalty--;
        return true;
    };

    public static EventDelegate PayRobber = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["gold"].GetQuantity() < 20)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("The robber demands 20 gold.");
            return false;
        }

        playerInventory.RemoveItem("gold", 20);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Lost 20 gold.");
        return true;
    };

    public static EventDelegate FightRobber = () => {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ChangeAp(-10);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Lost 10 AP.");
        return true;
    };
}
