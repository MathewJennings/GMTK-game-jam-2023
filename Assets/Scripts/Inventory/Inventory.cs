using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, Item> inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new Dictionary<string, Item>();
        
        // Get all items.
        GameObject itemManager = GameObject.Find("/ItemManager");
        List<Item> allItems = itemManager.GetComponent<AllItems>().GetAllItems();
        
        // Populate items with 0 of each item.
        foreach(Item item in allItems)
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
