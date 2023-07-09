using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    public Seed seed;
    public bool isWatered;
    public float timePlanted = 0f;
    public float timeWatered = 0f;
    public float timeOutOfWater = 0f;
    public SpriteRenderer spriteRenderer;
    public Sprite barren;
    public Sprite seeded;
    public Sprite watered;
    public Sprite seededAndWatered;
    public SpriteRenderer sproutRenderer;
    public DayTimeController dayTimeController;

    private float timeSpentWatered;
    private EventManager eventManager;
    private static bool firstCropDried;
    // Start is called before the first frame update
    public void Start()
    {
        dayTimeController = GameObject.FindObjectOfType<DayTimeController>();
        eventManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
        seed = null;
        isWatered = false;
        timePlanted = 0f;
        timeOutOfWater = 0f;
        timeWatered = 0f;
        updateSprite(barren);
        removePlant();
    }

    private void FixedUpdate()
    {
        if (!dayTimeController.isTimePaused)
        {
            checkWater();
            if (seed != null)
            {
                checkDead();
                checkMature();

            }
        }
    }


    public void highlight()
    {
        spriteRenderer.color = Color.yellow;
    }

    public void unhilight()
    {
        spriteRenderer.color = Color.white;
    }
    private void checkWater()
    {
        if (isWatered)
        {
            timeSpentWatered += Time.deltaTime;
            if (outOfWater())
            {
                isWatered = false;
                timeOutOfWater = dayTimeController.getCurrentTimeSeconds();
                updateSprite(seed == null ? barren : seeded);
                if (!firstCropDried)
                {
                    firstCropDried = true;
                    eventManager.PrintResult("The water evaporated due to the heat.", 3f);
                    eventManager.PrintResultAfterDelay(3f, "Better water it again if you hope to ever harvest it. (E)", 3f);
                }
            }
        }
    }

    private bool outOfWater()
    {
        return dayTimeController.getCurrentTimeSeconds() >= timeWatered + seed.getWaterDurationTime();
    }

    private void checkDead()
    {
        if (!isWatered && outOfTime())
        {
            killPlot();
        }
    }

    private bool outOfTime()
    {
        return dayTimeController.getCurrentTimeSeconds() >= timeOutOfWater + seed.getDryToleranceTime();
    }

    private void checkMature()
    {
        if (seed != null && isMature())
        {
            growPlant();
        }
    }

    public void updateSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void growPlant()
    {
        sproutRenderer.sprite = seed.sproutSprite;
    }

    public void removePlant()
    {
        sproutRenderer.sprite = null;
    }

    public void plantSeed(Seed seed)
    {
        // Update Sprite on planting
        Debug.Log(seed);
        this.timePlanted = dayTimeController.getCurrentTimeSeconds();
        this.timeOutOfWater = dayTimeController.getCurrentTimeSeconds();
        this.timeSpentWatered = 0;
        this.seed = seed;
        updateSprite(isWatered ? seededAndWatered : seeded);
    }

    public void waterPlot()
    {
        // Update Sprite on watering
        this.isWatered = true;
        this.timeWatered = dayTimeController.getCurrentTimeSeconds();
        updateSprite(isEmpty() ? watered : seededAndWatered);
    }

    public void unwaterPlot()
    {
        this.isWatered = false;
        this.timeOutOfWater = dayTimeController.getCurrentTimeSeconds();
        updateSprite(isEmpty() ? barren : seeded);
    }

    public Item harvest()
    {
        // Update sprite when harvested
        Item yield = Instantiate(seed.yield);
        yield.SetQuantity(seed.yieldQuantity);
        seed = null;
        updateSprite(isWatered ? watered : barren);
        removePlant();
        return yield;
    }

    public void killPlot()
    {
        // Update Sprite when killed
        this.isWatered = false;
        updateSprite(barren);
        removePlant();
        if (seed != null)
        {
            eventManager.PrintResult("Your " + seed.gameObject.GetComponent<Item>().GetItemId() + " dried out and shriveled in the unrelenting sun.");
        }
        this.seed = null;
    }

    public bool needsWatering()
    {
        return !isEmpty() && !this.isWatered;
    }

    public bool isEmpty()
    {
        return this.seed == null;
    }

    public bool isMature()
    {
        Debug.Log(timeSpentWatered + "/" + seed.getMaturationTime());
        return timeSpentWatered >= seed.getMaturationTime();
    }
}
