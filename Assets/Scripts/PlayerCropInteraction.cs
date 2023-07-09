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
    private SummaryManager summaryManager;
    //Summary in the format <cropName, countOfCropHarvested>
    private Dictionary<string, int> cropSummary;
    //Summary in the format <cropName, countOfCropHarvested>
    private Dictionary<string, int> eatenSummary;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GetComponent<InventoryUI>();
        playerInventory = GetComponent<Inventory>();
        summaryManager = FindAnyObjectByType<SummaryManager>(); 
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
                Debug.Log("You are out of energy and can not perform that action!");
            }

        } else if (plot.isMature())
        {
            if (playerManager.canAffordAction(3))
            {
                playerManager.ChangeAp(-3);
                Item yield = plot.harvest();
                playerInventory.AddItem(yield.GetItemId(), yield.GetQuantity());
                //STICK IT IN THE INVENTORY!!!!!
                //add it to your summary
                InitializeCropSummaryIfNotExist();
                if (!cropSummary.ContainsKey(yield.name))
                {
                    cropSummary.Add(yield.name, yield.GetQuantity());
                } else
                {
                    cropSummary[yield.name] += yield.GetQuantity();
                }
            } else
            {
                Debug.Log("You are out of energy and can not perform that action!");
            }
        }
    }

    private void InitializeCropSummaryIfNotExist()
    {
        //Set up summary
        Dictionary<SummaryManager.SummaryType, object> summary = summaryManager.summary;
        if (!summary.ContainsKey(SummaryManager.SummaryType.CROP))
        {
            summary.Add(SummaryManager.SummaryType.CROP, new Dictionary<string, int>());
            cropSummary = (Dictionary<string, int>)summary[SummaryManager.SummaryType.CROP];
        }
    }
    private void InitializeEatenSummaryIfNotExist()
    {
        //Set up summary
        Dictionary<SummaryManager.SummaryType, object> summary = summaryManager.summary;
        if (!summary.ContainsKey(SummaryManager.SummaryType.EATEN))
        {
            summary.Add(SummaryManager.SummaryType.EATEN, new Dictionary<string, int>());
            eatenSummary = (Dictionary<string, int>)summary[SummaryManager.SummaryType.EATEN];
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
            } else
            {
                Debug.Log("You are out of energy and can not perform that action!");
            }
        }
    }

    public void eatCrop(Item item, Crop crop) {
        playerManager.ChangeHunger(crop.sustenance);
        playerInventory.RemoveItem(item.GetItemId(), 1);
        InitializeEatenSummaryIfNotExist();
        if (!eatenSummary.ContainsKey(crop.name))
        {
            eatenSummary.Add(crop.name, 1);
        }
        else
        {
            eatenSummary[crop.name] += 1;
        }
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
