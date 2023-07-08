using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Item :MonoBehaviour
{
    public enum ItemType
    {
        Gold,
        Seed,
        Crop,
    }

    [SerializeField] string itemId;
    [SerializeField] ItemType itemType;
    [SerializeField] int quantity;
    [SerializeField] int price;
    [SerializeField] string description;
    [SerializeField] string correspondingId;
    [SerializeField] Sprite inventoryIcon;
    [SerializeField] Seed seed;
    [SerializeField] Crop crop;

    public Item(string itemId, ItemType itemType, int quantity,
                int price, string description, Sprite inventoryIcon, string correspondingId = "", Seed seed = null, Crop crop = null)
    {
        this.itemId = itemId;
        this.itemType = itemType;
        this.quantity = quantity;
        this.price = price;
        this.description = description;
        this.inventoryIcon = inventoryIcon;
        this.correspondingId = correspondingId;
        this.seed = seed;
        this.crop = crop;
    }

    public Item(Item item)
    {
        this.itemId = item.itemId;
        this.itemType = item.itemType;
        this.quantity = item.quantity;
        this.price = item.price;
        this.description = item.description;
        this.inventoryIcon = item.inventoryIcon;
        this.correspondingId = item.correspondingId;
        this.seed = item.seed;
        this.crop = item.crop;
    }

    public override string ToString()
    {
        return itemId;
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

    public Sprite GetInventoryIcon()
    {
        return inventoryIcon;
    }

    public Seed GetSeed()
    {
        return seed;
    }

    public Crop GetCrop()
    {
        return crop;
    }

}
