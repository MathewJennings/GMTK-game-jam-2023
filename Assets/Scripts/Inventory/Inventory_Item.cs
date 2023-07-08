using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Inventory_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] TextMeshProUGUI quantityText;

    private InventoryUI inventoryUI;
    private UIInputManager uiInputManager;
    private PlayerCropInteraction playerCropInteraction;
    private BarterManager barterManager;

    private Item item;

    private void Start()
    {
        uiInputManager = GameObject.FindGameObjectWithTag("UIInputManager").GetComponent<UIInputManager>();
        barterManager = GameObject.FindGameObjectWithTag("BarterManager").GetComponent<BarterManager>();
    }

    public void setInventoryUI(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
        playerCropInteraction = inventoryUI.gameObject.GetComponent<PlayerCropInteraction>(); // Only the Player's instance should successfully find this
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryUI.UpdateItemDescription(item);
        inventoryUI.itemName.gameObject.SetActive(true);
        inventoryUI.itemDescription.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.itemName.gameObject.SetActive(false);
        inventoryUI.itemDescription.gameObject.SetActive(false);
    }

    public void setItem(Item item) {
        this.item = item;
        quantityText.text = item.GetQuantity().ToString();
    }

    public string getItemId()
    {
        return item.GetItemId();
    }

    public bool isPlayerItem()
    {
        return playerCropInteraction != null;
    }

    public void OnClick()
    {
        Item.ItemType itemType = item.GetItemType();
        if (uiInputManager.GetClickedGameObjects().Contains(this.gameObject))
        {
            if (barterManager.IsTrading() && itemType != Item.ItemType.Gold)
            {
                barterManager.setBarteringItem(this);
            }
            else if (Item.ItemType.Seed == itemType)
            {
                PlantSeed();
            }
            else if (Item.ItemType.Crop == itemType)
            {
                EatCrop();
            }
        }
    }

    private void PlantSeed()
    {
        Seed seed = item.GetSeed();
        if (seed != null)
        {
            playerCropInteraction.plantSeed(item, seed);
            inventoryUI.CloseInventory();
        }
    }

    private void EatCrop()
    {
        Crop crop = item.GetCrop();
        if (crop != null)
        {
            playerCropInteraction.eatCrop(item, crop);
            inventoryUI.CloseInventory();
        }
    }
}
