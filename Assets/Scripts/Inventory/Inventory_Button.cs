using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_Button : MonoBehaviour, IPointerEnterHandler
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
    }

}
