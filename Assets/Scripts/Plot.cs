using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    public Seed seed;
    public bool isWatered;
    public float timePlanted = 0f;
    public SpriteRenderer spriteRenderer;
    public Sprite barren;
    public Sprite seeded;
    public Sprite watered;
    public Sprite seededAndWatered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This needs to check to see if it's time to harvest. Are we just doing this once a day?
    }

    public void updateSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void plantSeed(Seed seed)
    {
        // Update Sprite on planting
        Debug.Log(seed);
        this.seed = seed;
        this.timePlanted = 0f;
        updateSprite(isWatered ? seededAndWatered : seeded);
    }

    public void waterPlot()
    {
        // Update Sprite on watering
        this.isWatered = true;
        updateSprite(isEmpty() ? watered : seededAndWatered);
    }
    public Yield harvest()
    {
        // Update sprite when harvested
        Yield yield = seed.yield;
        seed = null;
        updateSprite(isWatered ? watered : barren);
        return yield;
    }

    public void killPlot()
    {
        // Update Sprite when killed
        this.isWatered = false;
        this.seed = null;
        updateSprite(barren);
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
}
