using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantStore : MonoBehaviour
{
    [SerializeField] private List<GameItemSO> sellableItems;
    [SerializeField] private List<GameItemSO> purchasableItems;
    
    public void PurchaseItem(GameItemSO item)
    {
        // add the item to the player's inventory and deduct currency
        InventoryManager.Instance.AddItemToInventory(item);
        Debug.Log("Purchased item: " + item.name);
    }
    public void SellItem(GameItemSO item)
    {
        //remove item from inventory and add currency to player
        InventoryManager.Instance.RemoveItemFromInventory(item);
        Debug.Log("Sold item: " + item.name);
    }
    public void DonateItem(GameItemSO item)
    {
        //remove item from inventory and add SYMPATHY POINTS to player
        InventoryManager.Instance.RemoveItemFromInventory(item);
        Debug.Log("Donated item: " + item.name);
    }
}
