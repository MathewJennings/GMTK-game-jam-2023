using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Globalization;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] GameObject inventoryBackground;
    [SerializeField] PlayerSounds playerSounds;
    [SerializeField] public TMP_Text itemName;
    [SerializeField] public TMP_Text itemDescription;
    [SerializeField] public Inventory_Item goldInventoryItem;

    private bool isOpen;
    private List<GameObject> currentInventoryItems = new List<GameObject>();
    private float inventoryImageWidth=100;

    public void OnCloseInventory()
    {
        CloseInventory();
    }
    public bool isInventoryOpen()
    {
        return isOpen;
    }

    public void OnOpenInventory()
    {
        if (isOpen)
        {
            CloseInventory();
        } else
        {
            OpenInventory();
        }
    }

    public void OpenInventory(bool playSound = true)
    {
        if (isOpen)
        {
            return;
        }
        if (playerSounds != null && playSound)
        {
            playerSounds.playOpenInventory();
        }
        inventoryBackground.SetActive(true);
        setupGoldInventoryItem();
        instantiateInventoryItems();
        ArrangeImagesHorizontally();
        isOpen = true;
    }

    private void setupGoldInventoryItem()
    {
        goldInventoryItem.setItem(inventory.inventory["gold"]);
        goldInventoryItem.setInventoryUI(this);
    }

    private void instantiateInventoryItems()
    {
        foreach (Item item in inventory.inventory.Values)
        {
            if (item.GetItemId() != "gold" && item.GetQuantity() != 0)
            {
                GameObject inventoryItem = Instantiate(inventoryItemPrefab);
                inventoryItem.transform.SetParent(inventoryBackground.transform, false);
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
    public void CloseInventory(bool playSound = true)
    {
        if(!isOpen)
        {
            return;
        }
        if (playerSounds != null && playSound)
        {
            playerSounds.playCloseInventory();
        }
        inventoryBackground.SetActive(false);
        foreach (GameObject inventoryItem in currentInventoryItems)
        {
            Destroy(inventoryItem);
        }
        currentInventoryItems.Clear();
        itemName.gameObject.SetActive(false);
        itemDescription.gameObject.SetActive(false);
        isOpen = false;
    }

    public void UpdateItemDescription(Item item, bool isTrading)
    {
        string description = item.GetItemId() + (isTrading ? " (" + item.GetPrice() + "G)" : "");
        itemName.text = new CultureInfo("en-US", false).TextInfo.ToTitleCase(description);
        itemDescription.text = item.GetDescription();
    }

    public void refresh()
    {
        setupGoldInventoryItem();
        CloseInventory(false);
        OpenInventory(false);
    }
}
