using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Contracts;
//using UnityEngine.UIElements;

public class Inventory_UI : MonoBehaviour
{
    private Inventory current_inventory;


  
    public AllItems allItems;
    private List<Item> allItemList;

    //item description
    public TMP_Text itemName;
    public TMP_Text itemQuantity;
    public TMP_Text itemDescription;


    //index for keeping current items
    private int totalItems;

    //actual display (one item)
    public Image inventory_image;
    //collection of all the displays
    private Image[] current_inventory_images;

    //collection of sprites for all the items
    public Sprite[] item_images;
    //how big the inventory is
    private float inventory_width=100; 

    //where the inventory should be.
    public Transform inventory_transform;

    public Canvas inventory_canvas;

    // Start is called before the first frame update
    void Start()
    {
        allItemList = allItems.GetAllItems();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenInventory()
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
                    }
                    else
                    {
                        current_inventory_images[totalItems_temp] = Instantiate(inventory_image, gameObject.transform);
                        current_inventory_images[totalItems_temp].transform.GetComponentInChildren<Inventory_Button>().inventory_UI = GetComponent<Inventory_UI>();
                        current_inventory_images[totalItems_temp].transform.GetComponentInChildren<Inventory_Button>().itemID = item.GetItemId();
                        
                        //current_inventory_images[totalItems_temp].transform.GetComponentInChildren<Button>().targetGraphic = item_images[];

                        current_inventory_images[totalItems_temp].transform.position = inventory_transform.position + new Vector3((totalItems - 1 + totalItems_temp)*(inventory_width*-0.5f), 0, 0);
                        current_inventory_images[totalItems_temp].transform.parent = inventory_canvas.transform;
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
    public void CloseInventory()
    {
        foreach (Image image in current_inventory_images)
        {
            if (image != null)
            {
                Destroy(image);
            }
        }
        current_inventory_images = null;
    }



}
