using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    private List<Item> allItems = new List<Item>
    {
        new Item("gold", Item.ItemType.Gold, 0, 1, "Dwarves love it, Goblins see it as a necessary evil, but humans will kill for it."),
        new Item("fooSeed", Item.ItemType.Seed, 0, 10, "Grows into a foo. What is a foo anyway?", "fooCrop"),
        new Item("fooCrop", Item.ItemType.Crop, 0, 10, "I see, so this is a foo.", "fooSeed"),
        new Item("barSeed", Item.ItemType.Seed, 0, 10, "Grows into a bar. Takes 3 days to grow", "barCrop"),
        new Item("barCrop", Item.ItemType.Crop, 0, 10, "Bars are high in protien, but also high in sugar. Great for a war torn hellscape no?", "barSeed"),
    };

    public List<Item> GetAllItems()
    {
        return allItems;
    }
}
