using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Globalization;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory player_inventory;
    [SerializeField] Image inventoryItemPrefab;
    [SerializeField] GameObject inventoryBackground;
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public TMP_Text itemDescription;
    [SerializeField] Transform inventoryParentTrasform;

    private bool isOpen;

    //collection of all the displays
    private List<Image> currentInventoryImages;

    //collection of sprites for all the items
    public Sprite[] item_images;
    //how big the inventory is
    private float inventory_width=100; 


    public bool isInventoryOpen()
    {
        return isOpen;
    }

    public void OpenInventory()
    {
        if(isOpen)
        {
            return;
        }
        inventoryBackground.SetActive(true);
        currentInventoryImages = new List<Image>();
        instantiateImages();
        ArrangeImagesHorizontally();
        isOpen = true;
    }

    private void instantiateImages()
    {
        foreach (Item item in player_inventory.inventory.Values)
        {
            if (item.GetQuantity() != 0)
            {
                Image image = Instantiate(inventoryItemPrefab);
                image.GetComponent<Inventory_Item>().setItem(item);
                image.sprite = item.GetInventoryIcon();
                image.transform.SetParent(inventoryParentTrasform, false);
                image.transform.position = new Vector3(Screen.width * 0.5f, 105, 0); ;
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
        inventoryBackground.SetActive(false);
        foreach (Image image in currentInventoryImages)
        {
            Destroy(image.gameObject);
        }
        currentInventoryImages.Clear();
        isOpen = false;
    }

    public void UpdateItemDescription(Item item)
    {
        itemName.text = new CultureInfo("en-US", false).TextInfo.ToTitleCase(item.GetItemId());
        itemDescription.text = item.GetDescription();
    }
}
