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

    private Item item;

    private void Start()
    {
        uiInputManager = GameObject.FindGameObjectWithTag("UIInputManager").GetComponent<UIInputManager>();
        playerCropInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCropInteraction>();
    }

    public void setInventoryUI(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
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

    public void OnClick()
    {
        if (uiInputManager.GetClickedGameObjects().Contains(this.gameObject))
        {
            Seed seed = item.GetSeed();
            if (seed != null)
            {
                playerCropInteraction.plantSeed(item, seed);
                inventoryUI.CloseInventory();
            }
        }
    }

}
