using System.Collections.Generic;
using UnityEngine;

public class PlayerCropInteraction : MonoBehaviour
{
    public Plot plot;
    public PlayerStats playerStats;

    private InventoryUI inventoryUI;
    private Inventory playerInventory;
    private PlayerSounds playerSounds;
    private EventManager eventManager;
    private bool plantedFirstSeed;
    private bool wateredFirstCrop;
    private bool harvestedFirstCrop;
    private SummaryManager summaryManager;
    private bool canDogPet;
    //Summary in the format <cropName, countOfCropHarvested>
    private Dictionary<string, int> cropSummary;
    //Summary in the format <cropName, countOfCropHarvested>
    private Dictionary<string, int> eatenSummary;

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GetComponent<InventoryUI>();
        playerInventory = GetComponent<Inventory>();
        eventManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
        summaryManager = FindAnyObjectByType<SummaryManager>(); 
        playerSounds = GetComponent<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUse()
    {
        serviceCrop();
        petDog();
    }

    private void petDog()
    {
        if (canDogPet) {
            Debug.Log("pet the dog");
        }
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
        else if (plot.needsWatering() && !plot.isMature())
        {
            int cost = 1;
            if (playerStats.canAffordAction(cost))
            {
                playerStats.ChangeAp(-1*cost);
                playerSounds.playWaterPlant();
                plot.waterPlot();
                if (!wateredFirstCrop)
                {
                    eventManager.PrintResult("Your back aches from watering the crop. (-" + cost + ")");
                    eventManager.PrintResultAfterDelay(2f, "It will probably be ready for harvest tomorrow...");
                    wateredFirstCrop = true;
                }
            } else
            {
                printOutOfEnergyMessage(cost);
            }

        } else if (plot.isMature())
        {
            int cost = 0;
            if (playerStats.canAffordAction(cost))
            {
                playerStats.ChangeAp(-1*cost);
                playerSounds.playHarvestPlant();
                Item yield = plot.harvest();
                playerInventory.AddItem(yield.GetItemId(), yield.GetQuantity());
                playerInventory.AddItem(yield.GetCorrespondingId(), 1); // yield 1 seed as well

                if(!harvestedFirstCrop)
                {
                    eventManager.PrintResult("You harvested a " + yield.GetItemId() + " and got a seed too. You're not sure if you should eat it or sell it.", 3f);
                    harvestedFirstCrop = true;
                }
                
                InitializeCropSummaryIfNotExist();
                if (!cropSummary.ContainsKey(yield.name))
                {
                    cropSummary.Add(yield.name, yield.GetQuantity());
                }
                else
                {
                    cropSummary[yield.name] += yield.GetQuantity();
                }
            } else
            {
                printOutOfEnergyMessage(cost);
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
            int cost = 1;
            if (playerStats.canAffordAction(cost))
            {
                playerStats.ChangeAp(-1*cost);
                playerSounds.playPlantSeed();
                plot.plantSeed(seed);
                playerInventory.RemoveItem(item.GetItemId(), 1);
                if (!plantedFirstSeed)
                {
                    eventManager.PrintResult("You planted your " + item.GetItemId() + ". You're always so tired now. (-" + cost + ")", 3f);
                    eventManager.PrintResultAfterDelay(3f, "Don't forget to water it (E)");
                    plantedFirstSeed = true;
                }
            }
            else
            {
                printOutOfEnergyMessage(cost);
            }
        }
    }

    private void printOutOfEnergyMessage(int necessaryAP)
    {
        eventManager.PrintResult("You are too tired to do that (need " + necessaryAP + " Energy)");
    }

    public void eatCrop(Item item, Crop crop) {
        playerSounds.playEatFood();
        playerStats.ChangeHunger(crop.sustenance);
        eventManager.PrintResult("The " + item.GetItemId() + " made you less hungry. (+" + crop.sustenance + ")", 3f);
        int energyChange = crop.sustenance;
        playerStats.ChangeAp(energyChange);
        eventManager.PrintResultAfterDelay(3f, "... You feel a bit more energized too. (+" + energyChange + ")", 3f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DogArea dogArea = collision.GetComponent<DogArea>();
        if (dogArea != null)
        {
            canDogPet = true;
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
        DogArea dogArea = collision.GetComponent<DogArea>();
        if (dogArea != null)
        {
            canDogPet = false;
        }
    }
}
