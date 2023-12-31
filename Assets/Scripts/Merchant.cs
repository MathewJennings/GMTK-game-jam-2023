using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    Inventory playerInventory;
    Inventory merchantInventory;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        merchantInventory = gameObject.GetComponent<Inventory>();
    }

    void SetPrice(string itemId,  int price)
    {
        Item item = merchantInventory.inventory[itemId];
        item.SetPrice(price);
    }

    public void PurchaseItem(string itemId)
    {
        Item playerGold = playerInventory.inventory["gold"];
        Item merchantGold = merchantInventory.inventory["gold"];
        Item playerItem = playerInventory.inventory[itemId];
        Item merchantItem = merchantInventory.inventory[itemId];
        if (merchantItem.GetQuantity() <= 0)
        {
            Debug.Log("Merchant does not have " + itemId);
            return;
        }

        if (playerGold.GetQuantity() < merchantItem.GetPrice())
        {
            Debug.Log("You do not have enough money");
            return;
        }

        playerGold.DecreaseQuantity(merchantItem.GetPrice());
        merchantGold.IncreaseQuantity(merchantItem.GetPrice());
        playerItem.IncreaseQuantity(1);
        merchantItem.DecreaseQuantity(1);
    }

    public void SellItem(string itemId)
    {
        Item playerGold = playerInventory.inventory["gold"];
        Item merchantGold = merchantInventory.inventory["gold"];
        Item playerItem = playerInventory.inventory[itemId];
        Item merchantItem = merchantInventory.inventory[itemId];
        if (playerItem.GetQuantity() <= 0)
        {
            Debug.Log("You do not have " + itemId + " to sell");
            return;
        }

        if (merchantGold.GetQuantity() < playerItem.GetPrice())
        {
            Debug.Log("Merchant does not have enough gold");
            return;
        }

        playerGold.IncreaseQuantity(playerItem.GetPrice());
        merchantGold.DecreaseQuantity(playerItem.GetPrice());
        merchantItem.IncreaseQuantity(1);
        playerItem.DecreaseQuantity(1);
    }

    [ContextMenu("BuyAppleSeed")]
    public void BuyAppleSeed()
    {
        PurchaseItem("appleSeed");
        Debug.Log("gold " + playerInventory.inventory["gold"].GetQuantity());
        Debug.Log("appleSeed " + playerInventory.inventory["appleSeed"].GetQuantity());
    }

    [ContextMenu("BuyAppleCrop")]
    public void BuyAppleCrop()
    {
        PurchaseItem("appleCrop");
        Debug.Log("gold " + playerInventory.inventory["gold"].GetQuantity());
        Debug.Log("appleCrop " + playerInventory.inventory["appleCrop"].GetQuantity());
    }

    [ContextMenu("SellAppleSeed")]
    public void SellAppleSeed()
    {
        SellItem("appleSeed");
        Debug.Log("gold " + playerInventory.inventory["gold"].GetQuantity());
        Debug.Log("appleSeed " + playerInventory.inventory["appleSeed"].GetQuantity());
    }

    [ContextMenu("SellAppleCrop")]
    public void SellAppleCrop()
    {
        SellItem("appleCrop");
        Debug.Log("gold " + playerInventory.inventory["gold"].GetQuantity());
        Debug.Log("appleCrop " + playerInventory.inventory["appleCrop"].GetQuantity());
    }
}
