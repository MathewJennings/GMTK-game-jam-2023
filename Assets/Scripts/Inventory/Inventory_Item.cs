using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Inventory_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] TextMeshProUGUI quantityText;

    private Inventory_UI inventory_UI;
    public Item item;

    private void Start()
    {
        inventory_UI = GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<Inventory_UI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory_UI.UpdateItemDescription(item);
        inventory_UI.itemName.gameObject.SetActive(true);
        inventory_UI.itemDescription.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventory_UI.itemName.gameObject.SetActive(false);
        inventory_UI.itemDescription.gameObject.SetActive(false);
    }

    public void setItem(Item item) {
        this.item = item;
        quantityText.text = item.GetQuantity().ToString();
    }

}
