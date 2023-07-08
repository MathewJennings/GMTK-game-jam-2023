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
    // Start is called before the first frame update
    void Start()
    {
        dayTimeController = GameObject.FindObjectOfType<DayTimeController>();
    }

    private void FixedUpdate()
    {
        if (seed != null)
        {
            checkWater();
            checkDead();
            checkMature();
        }
    }

    private void checkWater()
    {
        if (isWatered && outOfWater())
        {
            isWatered = false;
            timeOutOfWater = dayTimeController.getCurrentTimeSeconds();
            updateSprite(seeded);
        }
    }

    private bool outOfWater()
    {
        return timeWatered + seed.getWaterDurationTime() <= dayTimeController.getCurrentTimeSeconds();
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
        return timeOutOfWater + seed.getDryToleranceTime() <= dayTimeController.getCurrentTimeSeconds();
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
        this.seed = null;
        updateSprite(barren);
        removePlant();
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
        Debug.Log(seed);
        return timePlanted + seed.getMaturationTime() <= dayTimeController.getCurrentTimeSeconds();
    }
}
