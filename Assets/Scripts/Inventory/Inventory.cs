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

    [ContextMenu("IncreaseTenGold")]
    public void AddTenGold()
    {
        AddItem("gold", 10);
        Debug.Log(inventory["gold"].GetQuantity());
    }

    [ContextMenu("RemoveSevenGold")]
    public void RemoveSevenGold()
    {
        RemoveItem("gold", 7);
        Debug.Log(inventory["gold"].GetQuantity());
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
}
