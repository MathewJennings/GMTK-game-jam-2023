using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCropInteraction : MonoBehaviour
{
    public Plot plot;
    public PlayerStats playerManager;

    private InventoryUI inventoryUI;
    private Inventory playerInventory;
    private EventManager eventManager;
    private bool plantedFirstSeed;
    private bool wateredFirstCrop;
    private bool harvestedFirstCrop;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GetComponent<InventoryUI>();
        playerInventory = GetComponent<Inventory>();
        eventManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUse()
    {
        Debug.Log("USE");
        serviceCrop();
    }

    private void serviceCrop()
    {
        if (plot == null)
        {
            return;
        }

        if (plot.isEmpty())
        {
            if (!inventoryUI.isInventoryOpen())
            {
                inventoryUI.OpenInventory(); // So that we can trigger plantSeed() from Inventory_Item.OnClick()
            }
        }
        else if (plot.needsWatering())
        {
            if (playerManager.canAffordAction(1))
            {
                playerManager.ChangeAp(-1);
                plot.waterPlot();
            } else
            {
                printOutOfEnergyMessage();
            }

        } else if (plot.isMature())
        {
            if (playerManager.canAffordAction(3))
            {
                playerManager.ChangeAp(-3);
                Item yield = plot.harvest();
                playerInventory.AddItem(yield.GetItemId(), yield.GetQuantity());
            } else
            {
                printOutOfEnergyMessage();
            }
        }
    }

    public void plantSeed(Item item, Seed seed)
    {
        if (plot == null)
        {
            return;
        }
        if (plot.isEmpty())
        {
            if (playerManager.canAffordAction(2))
            {
                playerManager.ChangeAp(-2);
                plot.plantSeed(seed);
                playerInventory.RemoveItem(item.GetItemId(), 1);
                if (!plantedFirstSeed)
                {
                    eventManager.PrintResult("You planted your " + item.GetItemId() + ". You're always so tired now. (-2)", 3f);
                    eventManager.PrintResultAfterDelay(3f, "Don't forget to water it (E)");
                    plantedFirstSeed = true;
                }
            }
            else
            {
                printOutOfEnergyMessage();
            }
        }
    }

    private void printOutOfEnergyMessage()
    {
        eventManager.PrintResult("You are too exhausted to do that (0 Action Points)");
    }

    public void eatCrop(Item item, Crop crop) {
        playerManager.ChangeHunger(crop.sustenance);
        playerInventory.RemoveItem(item.GetItemId(), 1);
        eventManager.PrintResult("The " + item.GetItemId() + " made you less hungry. (+" + crop.sustenance + ")", 3f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Plot collidedPlot = collision.gameObject.GetComponent<Plot>();
        if (collidedPlot != null)
        {
            selectPlot(collidedPlot);
            plot = collidedPlot;
        }
    }

    private void selectPlot(Plot selectedPlot)
    {
        if (plot != null && selectedPlot != plot)
        {
            plot.unhilight();
            selectedPlot.highlight();
            plot = selectedPlot;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Plot collidedPlot = collision.gameObject.GetComponent<Plot>();
        if (collidedPlot == plot && plot != null)
        {
            plot.unhilight();
            plot = null;
        }
    }
}
