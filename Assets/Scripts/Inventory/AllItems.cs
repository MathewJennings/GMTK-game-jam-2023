using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    private List<Item> allItems = new List<Item>
    {
        new Item("gold", Item.ItemType.Gold, 0, 1),
        new Item("fooSeed", Item.ItemType.Seed, 0, 10, "fooCrop"),
        new Item("fooCrop", Item.ItemType.Crop, 0, 10, "fooSeed"),
        new Item("barSeed", Item.ItemType.Seed, 0, 10, "barCrop"),
        new Item("barCrop", Item.ItemType.Crop, 0, 10, "barSeed"),
    };

    public List<Item> GetAllItems()
    {
        return allItems;
    }
}
