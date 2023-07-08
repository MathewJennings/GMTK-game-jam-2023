using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Globalization;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] GameObject inventoryBackground;
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public TMP_Text itemDescription;

    private bool isOpen;
    private List<GameObject> currentInventoryItems = new List<GameObject>();
    private float inventoryImageWidth=100; 


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
        instantiateInventoryItems();
        ArrangeImagesHorizontally();
        isOpen = true;
    }

    private void instantiateInventoryItems()
    {
        foreach (Item item in inventory.inventory.Values)
        {
            if (item.GetQuantity() != 0)
            {
                GameObject inventoryItem = Instantiate(inventoryItemPrefab);
                inventoryItem.transform.SetParent(inventoryBackground.transform, false);
                inventoryItem.transform.position = new Vector3(Screen.width * 0.5f, 105, 0);
                currentInventoryItems.Add(inventoryItem);
                Inventory_Item inventoryItemScript = inventoryItem.GetComponent<Inventory_Item>();
                inventoryItemScript.setInventoryUI(this);
                inventoryItemScript.setItem(item);
                Image image = inventoryItem.GetComponent<Image>();
                image.sprite = item.GetInventoryIcon();
            }
        }
    }

    private void ArrangeImagesHorizontally()
    {
        int totalImageWidth = 100 * currentInventoryItems.Count;
        for (int i = 0; i < currentInventoryItems.Count; i++)
        {
            currentInventoryItems[i].transform.Translate(-1 * totalImageWidth / 2 + inventoryImageWidth * i + 50, 0, 0);
        }
    }
    public void CloseInventory()
    {
        if(!isOpen)
        {
            return;
        }
        inventoryBackground.SetActive(false);
        foreach (GameObject inventoryItem in currentInventoryItems)
        {
            Destroy(inventoryItem);
        }
        currentInventoryItems.Clear();
        isOpen = false;
    }

    public void UpdateItemDescription(Item item)
    {
        itemName.text = new CultureInfo("en-US", false).TextInfo.ToTitleCase(item.GetItemId());
        itemDescription.text = item.GetDescription();
    }
}
