using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory_UI : MonoBehaviour
{
    [SerializeField] Inventory player_inventory;
    [SerializeField] Image inventoryItemPrefab;
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public TMP_Text itemQuantity;
    [SerializeField] public TMP_Text itemDescription;

    private bool isOpen;

    //collection of all the displays
    private List<Image> currentInventoryImages;

    //collection of sprites for all the items
    public Sprite[] item_images;
    //how big the inventory is
    private float inventory_width=100; 

    //where the inventory should be.
    public Transform inventory_transform;

    public Canvas inventory_canvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            toggleInventory();
        }
    }

    private void toggleInventory()
    {
        if (isOpen)
        {
            CloseInventory();
        } else
        {
            OpenInventory();
        }
        isOpen = !isOpen;
    }

    public void OpenInventory()
    {
        currentInventoryImages = new List<Image>();
        instantiateImages();
        ArrangeImagesHorizontally();
    }

    private void instantiateImages()
    {
        foreach (Item item in player_inventory.inventory.Values)
        {
            if (item.GetQuantity() != 0)
            {
                Image image = Instantiate(inventoryItemPrefab);
                image.transform.SetParent(inventory_canvas.transform, false);
                image.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0); ;
                image.gameObject.GetComponentInChildren<Inventory_Item>().itemID = item.GetItemId();
                currentInventoryImages.Add(image);
            }
        }
    }

    private void ArrangeImagesHorizontally()
    {
        int totalImageWidth = 100 * currentInventoryImages.Count;
        for (int i = 0; i < currentInventoryImages.Count; i++)
        {
            currentInventoryImages[i].transform.Translate(-1 * totalImageWidth / 2 + 100 * i + 50, 0, 0);
        }
    }
    public void CloseInventory()
    {
        foreach (Image image in currentInventoryImages)
        {
            Destroy(image.gameObject);
        }
    }

    public void UpdateItemDescription(string itemID)
    {
        itemName.text = player_inventory.inventory[itemID].GetItemId();
        itemQuantity.text = player_inventory.inventory[itemID].GetQuantity()+"";
        itemDescription.text = player_inventory.inventory[itemID].GetDescription();
    }
}
