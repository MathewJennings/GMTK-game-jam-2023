using System.Collections;
using System.Collections.Generic;
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
    private string correspondingId;

    public Item(string itemId, ItemType itemType, int quantity,
                string correspondingId = "")
    {
        this.itemId = itemId;
        this.itemType = itemType;
        this.quantity = quantity;
        this.correspondingId = correspondingId;
    }

    public Item(Item item)
    {
        this.itemId = item.itemId;
        this.itemType = item.itemType;
        this.quantity = item.quantity;
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

    public string GetCorrespondingId()
    {
        return correspondingId;
    }
}
