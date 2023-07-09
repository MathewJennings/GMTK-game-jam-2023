using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, Item> inventory;

    public int startGold;
    public int startAppleSeed;
    public int startAppleCrop;
    public int startCarrotSeed;
    public int startCarrotCrop;

    public int priceAppleSeed;
    public int priceAppleCrop;
    public int priceCarrotSeed;
    public int priceCarrotCrop;

    // Start is called before the first frame update
    public void Start()
    {
        InitEmptyInventory();

        inventory["gold"].SetQuantity(startGold);
        inventory["appleSeed"].SetQuantity(startAppleSeed);
        inventory["appleSeed"].SetPrice(priceAppleSeed);
        inventory["appleCrop"].SetQuantity(startAppleCrop);
        inventory["appleCrop"].SetPrice(priceAppleCrop);
        inventory["carrotSeed"].SetQuantity(startCarrotSeed);
        inventory["carrotSeed"].SetPrice(priceCarrotSeed);
        inventory["carrotCrop"].SetQuantity(startCarrotCrop);
        inventory["carrotCrop"].SetPrice(priceCarrotCrop);
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

    public void ClearItem(string itemId)
    {
        Item item = inventory[itemId];
        item.SetQuantity(0);
    }

    public void Clear()
    {
        foreach(string itemId in inventory.Keys)
        {
            inventory[itemId].SetQuantity(0);
        }
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
