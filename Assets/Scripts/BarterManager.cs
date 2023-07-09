using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarterManager : MonoBehaviour
{
    [SerializeField] Button barterButton;
    [SerializeField] TMP_Text barterButtonText;

    private bool finishedFirstTrade;
    private bool isTrading;
    private bool playerIsBuying;
    private Inventory_Item selectedBarteringInventoryItem;
    private GameObject merchant;
    private GameObject player;
    private PlayerSounds playerSounds;
    private EventManager eventManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSounds = player.GetComponent<PlayerSounds>();
        eventManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EventManager>();
    }

    public void setBarteringItem(Inventory_Item inventoryItem)
    {
        selectedBarteringInventoryItem = inventoryItem;
        barterButton.gameObject.SetActive(true);
        playerIsBuying = !selectedBarteringInventoryItem.isPlayerItem();
        string barterText = (playerIsBuying ? "Buy" : "Sell") + " 1 " + inventoryItem.getItemId() + " for " + inventoryItem.getItemPrice() + "G?";
        barterButtonText.text = barterText;
    }

    public bool IsTrading()
    {
        return isTrading;
    }

    public void startTrading(GameObject merchant)
    {
        this.merchant = merchant;
        merchant.GetComponent<InventoryUI>().OpenInventory();
        player.GetComponent<InventoryUI>().OpenInventory();
        isTrading = true;
    }

    public void stopTrading()
    {
        player.GetComponent<InventoryUI>().CloseInventory();
        merchant.GetComponent<InventoryUI>().CloseInventory();
        merchant = null;
        isTrading = false;
        barterButton.gameObject.SetActive(false);
        selectedBarteringInventoryItem = null;

        if (!finishedFirstTrade)
        {
            finishedFirstTrade = true;
            eventManager.PrintResultAfterDelay(0.5f, "You need to grow more food on your field. (E)");
        }
    }
    
    public void performTrade()
    {
        Merchant merchantScript = merchant.GetComponent<Merchant>();
        if (playerIsBuying)
        {
           merchantScript.PurchaseItem(selectedBarteringInventoryItem.getItemId());
        }
        else
        {
            merchantScript.SellItem(selectedBarteringInventoryItem.getItemId());
        }
        playerSounds.playBuySell();
        player.GetComponent<InventoryUI>().refresh();
        merchant.GetComponent<InventoryUI>().refresh();
    }
}
