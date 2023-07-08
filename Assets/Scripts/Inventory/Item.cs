using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Item
{
    public enum ItemType
    {
        Gold,
        Seed,
        Crop,
    }

    private string itemId;
    private ItemType itemType;
    private int quantity;
    private int price;
    private string description { get; set; }
    private string correspondingId;

    public Item(string itemId, ItemType itemType, int quantity,
                int price, string description, string correspondingId = "")
    {
        this.itemId = itemId;
        this.itemType = itemType;
        this.quantity = quantity;
        this.price = price;
        this.description = description;
        this.correspondingId = correspondingId;
    }

    public Item(Item item)
    {
        this.itemId = item.itemId;
        this.itemType = item.itemType;
        this.quantity = item.quantity;
        this.price = item.price;
        this.description = item.description;
        this.correspondingId = item.correspondingId;
    }

    public string GetItemId()
    {
        return itemId;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void SetQuantity(int i)
    {
        if (i >= 0)
        {
            quantity = i;
        }
    }

    public void IncreaseQuantity(int i)
    {
        quantity += i;
    }

    public void DecreaseQuantity(int i)
    {
        if (quantity >= i)
        {
            quantity -= i;
        }
        else
        {
            quantity = 0;
        }
    }

    public int GetPrice()
    {
        return price;
    }

    public void SetPrice(int p)
    {
        if (price > 0)
        {
            price = p;
        }
    }

    public string GetCorrespondingId()
    {
        return correspondingId;
    }
    public string GetDescription()
    {
        return description;
    }

}
