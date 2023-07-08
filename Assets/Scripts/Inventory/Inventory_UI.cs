using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;

public class Inventory_UI : MonoBehaviour
{
    public int temp;
    public Inventory current_inventory;
    //public Button[] items;
    public AllItems allItems;
    public List<Item> allItemList;


    public TMP_Text itemName;
    public TMP_Text itemQuantity;
    public TMP_Text itemDescription;

    public int totalItems;
    public Image inventory_image;
    public Image[] current_inventory_images;

    public Transform inventory_position;

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemQuantity();
        allItemList = allItems.GetAllItems();


    }

    // Update is called once per frame
    void Update()
    {

        UpdateItemQuantity();
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
                        //items[int.Parse(item.GetItemId())].interactable = false;
                    }
                    else
                    {
                        totalItems++;
                    }
                }
            }

        }
        current_inventory_images=new Image[totalItems];
        int totalItems_temp = 0;
        foreach (Item item in current_inventory.inventory.Values)
        {

            foreach (Item item2 in allItemList)
            {
                if (item.GetItemId() == item2.GetItemId())
                {
                    if (item.GetQuantity() == 0)
                    {
                        //items[int.Parse(item.GetItemId())].interactable = false;
                    }
                    else
                    {
                        
                        current_inventory_images[totalItems_temp] = Instantiate(inventory_image, gameObject.transform);
                        current_inventory_images[totalItems_temp].transform.GetComponentInChildren<Inventory_Button>().inventory_UI = GetComponent<Inventory_UI>();
                        current_inventory_images[totalItems_temp].transform.GetComponentInChildren<Inventory_Button>().itemID = item.GetItemId();
                        current_inventory_images[totalItems_temp].transform.position = gameObject.transform.position + new Vector3(totalItems - 1 + (totalItems_temp*(-50)), 0, 0);
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
