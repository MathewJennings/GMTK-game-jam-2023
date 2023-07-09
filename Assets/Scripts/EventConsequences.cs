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
        int currGoldQuantity = playerInventory.inventory["gold"].GetQuantity();
        if (currGoldQuantity < 1)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have any gold to give.");
            return false;
        }
        int goldToGive = currGoldQuantity >= 5 ? 5 : currGoldQuantity;

        playerInventory.RemoveItem("gold", goldToGive);
        EventManager.philanthropic++;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult(goldToGive == 5 ? "You gave her the 5 gold she asked for." : "You gave her what you had.");
        return true;
    };

    public static EventDelegate sendGrandmaAway = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 10);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You grit your teeth and send her away. You can barely manage for yourself these days.", 5f);
        EventManager.refugee_denied_count++;
        return true;
    };
    public static EventDelegate giveFood = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        int numCarrots = playerInventory.inventory["carrotCrop"].GetQuantity();
        int numApples = playerInventory.inventory["appleCrop"].GetQuantity();
        int numGiven = 0;
        if (numCarrots >= 2)
        {
            playerInventory.RemoveItem("carrotCrop", 2);
            numGiven += 2;
        } else if (numCarrots == 1)
        {
            playerInventory.RemoveItem("carrotCrop", 1);
            numGiven += 1;
        }
        if (numGiven < 2)
        {
            if (numGiven == 0 && numApples >= 2)
            {
                playerInventory.RemoveItem("appleCrop", 2);
                numGiven += 2;
            } else if (numApples > 0)
            {
                playerInventory.RemoveItem("appleCrop", 1);
                numGiven += 1;
            }
        }
        if (numGiven == 0)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have any food to spare.");
            return false;
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You gave the poor soldier what you could (" + numGiven + ").", 5f);
        EventManager.philanthropic++;
        EventManager.human_loyalty++;
        return true;
    };
    public static EventDelegate reportHumanSoldier = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        EventManager.goblin_loyalty++;
        playerInventory.AddItem("gold", 2);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("After he leaves you tell the army about him. They plan to hunt him down, and pay you 2 gold for the information.", 5f);
        EventManager.refugee_denied_count++;
        EventManager.goblin_loyalty++;
        return true;
    };
    public static EventDelegate GoblinSoldier_OfferInventory = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        playerInventory.Clear();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You placate them by giving them everything you have, but at least they didn't assault you.", 5f);
        return true;


    };
    public static EventDelegate GoblinSoldier_Assault = () => {
        int energyCost = -7;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().ChangeAp(energyCost);
        player.GetComponent<PlayerMovement>().CrippleMovement();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("They assault you all at once, leaving you bloodied and broken. (" + energyCost + ")", 5f);
        EventManager.goblin_loyalty--;
        return true;
    };
    public static EventDelegate PayTax = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        if (playerInventory.inventory["gold"].GetQuantity() < 5)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("They demand more than that (5G).");
            return false;
        }

        playerInventory.RemoveItem("gold", 5);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("They extorted you, but at least you're not hurt.", 5f);
        EventManager.goblin_loyalty++;
        return true;
    };
    public static EventDelegate NotPayTax = () => {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Inventory playerInventory = player.GetComponent<Inventory>();
        playerInventory.Clear();
        player.GetComponent<PlayerStats>().ChangeAp(-3);
        player.GetComponent<PlayerMovement>().CrippleMovement();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
        .PrintResult("The mercenaries won't let you get away with that. They take pleasure in assaulting you (-3) and stealing what you have.", 5f);
        EventManager.goblin_loyalty--;
        return true;
    };

    public static EventDelegate PayRobber = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        int goldToGive = playerInventory.inventory["gold"].GetQuantity();
        if (goldToGive > 20)
        {
            goldToGive = 20;
        }
        else if (goldToGive < 1)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You don't have any gold to give.");
            return false;
        }

        playerInventory.RemoveItem("gold", goldToGive);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("The robber extorted you for " + goldToGive + " gold. At least you weren't assaulted.");
        return true;
    };

    public static EventDelegate AttackedByRobber = () => {
        int energyCost = -5;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().ChangeAp(energyCost);
        player.GetComponent<PlayerMovement>().CrippleMovement();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Clearly upset, the robber thrashed you before leaving you bloodied and bruised (" + energyCost + ")");
        return true;
    };

    public static EventDelegate OpenChest = () =>
    {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerInventory.AddItem("gold", 20);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("The chest contained 20 gold, a small fortune these days. Your curiosity was rewarded.", 5f);
        EventManager.treasure_count++;
        return true;
    };

    public static EventDelegate IgnoreChest = () =>
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You let the chest go, unsure of the nature of its magic.", 5f);
        return true;
    };

    public static EventDelegate OpenMimicChest = () => {
        int energyCost = -5;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().ChangeAp(energyCost);
        player.GetComponent<PlayerMovement>().CrippleMovement();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Your luck has turned, this chest was a mimic! It takes a bite out of you before running away (" + energyCost + ").", 5f);
        return true;
    };

    public static EventDelegate TreasureOwnerReturnMoney = () => {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        int goldToGive = playerInventory.inventory["gold"].GetQuantity();
        if (goldToGive > 20)
        {
            goldToGive = 20;
        } else if (goldToGive < 1)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You don't have any gold to return to the mage.");
            return false;
        }

        playerInventory.RemoveItem("gold", goldToGive);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Fearing for your life, you meekly return " + goldToGive + " gold. Satisfied, he leaves. You stop holding your breath.", 5f);
        EventManager.human_loyalty++;
        return true;
    };

    public static EventDelegate TreasureOwnerSayNo = () => {
        EventManager.philanthropic--;
        int energyCost = -10;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerStats>().ChangeAp(energyCost);
        player.GetComponent<Inventory>().Clear();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Displeased, the mage calls down a bolt of lightning on you and robs you while you lay there stunned (" + energyCost + ").", 5f);
        return true;
    };

    public static EventDelegate Rain = () =>
    {
        GameObject farmPlots = GameObject.Find("/GameManager/FarmPlots");
        if (farmPlots == null)
        {
            return true;
        }
        foreach (Plot p in farmPlots.GetComponentsInChildren<Plot>())
        {
            p.waterPlot();
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Your whole field has been watered by the downpour.", 5f);
        return true;
    };

    public static EventDelegate Drought = () =>
    {
        GameObject farmPlots = GameObject.Find("/GameManager/FarmPlots");
        if (farmPlots == null)
        {
            return true;
        }
        foreach (Plot p in farmPlots.GetComponentsInChildren<Plot>())
        {
            p.unwaterPlot();
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Your whole field has dried up. Better rewater the crops before they dry out.", 5f);
        return true;
    };

    public static EventDelegate Explosion = () =>
    {
        GameObject farmPlots = GameObject.Find("/GameManager/FarmPlots");
        if (farmPlots == null)
        {
            return true;
        }
        foreach (Plot p in farmPlots.GetComponentsInChildren<Plot>())
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            if (r > 0.5f) {
                p.killPlot();
            }
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("Your field has been partially destroyed. You'll have to plant new crops.", 5f);
        return true;
    };

    public static EventDelegate SupportHumanVictory = () =>
    {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        int numCarrots = playerInventory.inventory["carrotCrop"].GetQuantity();
        int numApples = playerInventory.inventory["appleCrop"].GetQuantity();
        int numGold = playerInventory.inventory["gold"].GetQuantity();
        if (numCarrots < 10 || numApples < 10 || numGold < 15)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have enough resources to meet the emissary's demands.");
            return false;
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You successfully supported the humans on the eve of the decisive battle. Their victory is all but assured.", 5f);
        EventManager.philanthropic++;
        EventManager.human_loyalty++;
        return true;
    };

    public static EventDelegate RejectHumanVictory = () =>
    {
        GameObject.FindAnyObjectByType<OverlayManager>().GameOverTransition(
                "You were killed",
                "Though you attempted to survive in this cruel wartorn land, it was not enough. You were immediately slain where you stood by the human emessary. Maybe a different goblin would fair better in the future.",
                true,
                "The Struggle Continues",
                () => GameObject.FindAnyObjectByType<RestartManager>().Restart()
            );
        return true;
    };

    public static EventDelegate SupportGoblinVictory = () =>
    {
        Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        int numCarrots = playerInventory.inventory["carrotCrop"].GetQuantity();
        int numApples = playerInventory.inventory["appleCrop"].GetQuantity();
        int numGold = playerInventory.inventory["gold"].GetQuantity();
        if (numCarrots < 10 || numApples < 10 || numGold < 15)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
                .PrintResult("You do not have enough resources to meet the emissary's demands.");
            return false;
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>()
            .PrintResult("You successfully supported the goblins on the eve of the decisive battle. Their victory is all but assured.", 5f);
        EventManager.philanthropic++;
        EventManager.human_loyalty++;
        return true;
    };

    public static EventDelegate RejectGoblinVictory = () =>
    {
        GameObject.FindAnyObjectByType<OverlayManager>().GameOverTransition(
                "You were killed",
                "Though you attempted to survive in this cruel wartorn land, it was not enough. You were immediately slain where you stood by the goblin emessary. Maybe a different goblin would fair better in the future.",
                true,
                "The Struggle Continues",
                () => GameObject.FindAnyObjectByType<RestartManager>().Restart()
            );
        return true;
    };
}
