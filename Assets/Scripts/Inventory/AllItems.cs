using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    private List<Item> allItems;

    // Start is called before the first frame update
    void Start()
    {
        allItems = new List<Item>
        {
            new Item("gold", Item.ItemType.Gold, 0),
            new Item("fooSeed", Item.ItemType.Seed, 0, "fooCrop"),
            new Item("fooCrop", Item.ItemType.Crop, 0, "fooSeed"),
            new Item("barSeed", Item.ItemType.Seed, 0, "barCrop"),
            new Item("barCrop", Item.ItemType.Crop, 0, "barSeed")
        };
    }

    public List<Item> GetAllItems()
    {
        return allItems;
    }
}
