using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{

    public Seed seed;
    public bool isWatered;
    public float timePlanted = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This needs to check to see if it's time to harvest. Are we just doing this once a day?
    }

    public void plantSeed(Seed seed)
    {
        // Update Sprite on planting
        this.seed = seed;
    }

    public void waterCrop()
    {
        // Update Sprite on watering
        this.isWatered = true;
    }

    public void killCrop()
    {
        // Update Sprite when killed
        this.isWatered = false;
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
        return timePlanted >= seed.maturationTime;
    }

    public Yield harvest()
    {
        // Update sprite when harvested
        return seed.yield;
    }
}
