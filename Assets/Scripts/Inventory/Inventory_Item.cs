using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Inventory_UI inventory_UI;
    public string itemID;

    private void Start()
    {
        inventory_UI = GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<Inventory_UI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory_UI.UpdateItemDescription(itemID);
        inventory_UI.itemName.gameObject.SetActive(true);
        inventory_UI.itemQuantity.gameObject.SetActive(true);
        inventory_UI.itemDescription.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventory_UI.itemName.gameObject.SetActive(false);
        inventory_UI.itemQuantity.gameObject.SetActive(false);
        inventory_UI.itemDescription.gameObject.SetActive(false);
    }

}
