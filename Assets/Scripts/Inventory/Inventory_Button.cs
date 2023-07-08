using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_UI inventory_UI;
    public string itemID;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory_UI.UpdateItemDescription(itemID);
        inventory_UI.itemName.gameObject.SetActive(true);
        inventory_UI.itemQuantity.gameObject.SetActive(true);
        inventory_UI.itemQuantity.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventory_UI.itemName.gameObject.SetActive(false);
        inventory_UI.itemQuantity.gameObject.SetActive(false);
        inventory_UI.itemQuantity.gameObject.SetActive(false);
    }

}
