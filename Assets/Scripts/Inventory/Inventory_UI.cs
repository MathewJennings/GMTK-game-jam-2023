using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Inventory_UI : MonoBehaviour
{
    public int temp;
    public Inventory current_inventory;
    public Button[] items;
    public AllItems allItems;
    public List<Item> allItemList;
    public TMP_Text itemName;
    public TMP_Text itemQuantity;
    public TMP_Text itemDescription;

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemQuantity();
        allItemList = allItems.GetAllItems();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateItemQuantity()
    {
        foreach (Item item in current_inventory.inventory.Values)
        {
            foreach(Item item2 in allItemList)
            { 
                if(item.GetItemId() == item2.GetItemId())
                {
                    if (item.GetQuantity() == 0)
                    {
                        items[int.Parse(item.GetItemId())].interactable = false;
                    }
                    else
                    {
                        items[int.Parse(item.GetItemId())].interactable = true;
                    }
                }
            }

        }
    }

    public void UpdateItemDescription(string itemID)
    {
        itemName.text = current_inventory.inventory[itemID].GetItemId();
        itemQuantity.text = current_inventory.inventory[itemID].GetQuantity()+"";
        itemDescription.text = current_inventory.inventory[itemID].GetDescription();
    }

}
