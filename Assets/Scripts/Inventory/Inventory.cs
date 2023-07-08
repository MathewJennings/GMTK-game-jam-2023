using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, Item> inventory;

    public int startGold;
    public int startFooSeed;
    public int startFooCrop;
    public int startBarSeed;
    public int startBarCrop;

    // Start is called before the first frame update
    void Start()
    {
        InitEmptyInventory();

        inventory["gold"].SetQuantity(startGold);
        inventory["fooSeed"].SetQuantity(startFooSeed);
        inventory["fooCrop"].SetQuantity(startFooCrop);
        inventory["barSeed"].SetQuantity(startBarSeed);
        inventory["barCrop"].SetQuantity(startBarCrop);
    }

    public void InitEmptyInventory()
    {
        inventory = new Dictionary<string, Item>();

        // Get all items.
        GameObject itemManager = GameObject.FindGameObjectWithTag("ItemManager");
        List<Item> allItems = itemManager.GetComponent<AllItems>().GetAllItems();

        // Populate items with 0 of each item.
        foreach (Item item in allItems)
        {
            inventory[item.GetItemId()] = new Item(item);
        }
    }

    public void AddItem(string itemId, int quantity)
    {
        Item item = inventory[itemId];
        item.IncreaseQuantity(quantity);
    }

    public void RemoveItem (string itemId, int quantity)
    {
        Item item = inventory[itemId];
        item.DecreaseQuantity(quantity);
    }

    public List<Item> GetSeeds()
    {
        List<Item> seeds = new List<Item>();

        foreach(Item item in inventory.Values)
        {
            if (item.GetItemType() == Item.ItemType.Seed)
            {
                seeds.Add(item);
            }
        }

        return seeds;
    }

    public List<Item> GetCrops()
    {
        List<Item> crops = new List<Item>();
        foreach (Item item in inventory.Values)
        {
            if (item.GetItemType() == Item.ItemType.Crop)
            {
                crops.Add(item);
            }
        }

        return crops;
    }
}
