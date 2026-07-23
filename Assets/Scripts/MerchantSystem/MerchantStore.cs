using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantStore : MonoBehaviour
{
    [SerializeField] private List<GameItemSO> sellableItems;
    [SerializeField] private List<GameItemSO> purchasableItems;
    [SerializeField] private MerchantUIManager uiManager;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buyItemButtonPrefab;    
    [SerializeField] private GameObject sellItemButtonPrefab;
    [SerializeField] private GameObject storeContent;

    [SerializeField] private OnClickButtonDisplay modeSwitchButtonDisplay;
    [SerializeField] private OnClickButtonDisplay itemListButtonDisplay;
    [SerializeField] private GameObject buyModeSwitchButton;
    // [SerializeField] private GameObject sellModeSwitchButton;
    public static bool merchantStoreOpen = false;
    private GameItemSO selectedItem;
    public GameItemSO GetSelectedItem()
    {
        return selectedItem;
    }
    public void SetSelectedItem(GameItemSO item)
    {
        selectedItem = item;
    }
    private GameObject selectedItemButton;
    public void SetSelectedItemButton(GameObject button)
    {
        selectedItemButton = button;
    }
    public GameObject GetSelectedItemButton()
    {
        return selectedItemButton;
    }

    public void PurchaseItem()
    {
        if (selectedItem.isVanityItem && selectedItem.cost <= CurrencyManager.Instance.GetCurrency())
        {
            if (VanityManager.Instance.HasVanityItem(selectedItem))
            {
                return; // already own this one
            }
            VanityManager.Instance.AddVanityItem(selectedItem);
            CurrencyManager.Instance.RemoveCurrency(selectedItem.cost);
            Debug.Log("Purchased vanity item: " + selectedItem.name);
            return;
        }
        // add the item to the player's inventory and deduct currency
        if (selectedItem.cost <= CurrencyManager.Instance.GetCurrency())
        {
            InventoryManager.Instance.AddItemToInventory(selectedItem);
            CurrencyManager.Instance.RemoveCurrency(selectedItem.cost);
            Debug.Log("Purchased item: " + selectedItem.name);
        }
    }
    public void SellItem()
    {
        //remove item from inventory and add currency to player
        InventoryManager.Instance.RemoveItemFromInventory(selectedItem);
        CurrencyManager.Instance.AddCurrency(selectedItem.cost);
        uiManager.UpdateRemovedItemButton();
        Debug.Log("Sold item: " + selectedItem.name);
    }
    public void DonateItem()
    {
        //remove item from inventory and add SYMPATHY POINTS to player
        InventoryManager.Instance.RemoveItemFromInventory(selectedItem);
        uiManager.UpdateRemovedItemButton();
        Debug.Log("Donated item: " + selectedItem.name);
    }

    public void PopulateBuyList()
    {
        ClearButtonContainer();
        foreach (GameItemSO item in purchasableItems)
        {
            GameObject buttonObj = Instantiate(buyItemButtonPrefab, buttonContainer);
            buttonObj.SetActive(true);
            BuyItemButtonUI buttonUI = buttonObj.GetComponent<BuyItemButtonUI>();
            buttonUI.SetUp(item, uiManager, this);
        }
        itemListButtonDisplay.UpdateButtonGroup(); //update the button group to include the newly created buttons
    }
    public void PopulateSellList()
    {
        ClearButtonContainer();
        foreach (GameItemSO item in sellableItems)
        {
            GameObject buttonObj = Instantiate(sellItemButtonPrefab, buttonContainer);
            buttonObj.SetActive(true);
            SellItemButtonUI buttonUI = buttonObj.GetComponent<SellItemButtonUI>();
            buttonUI.SetUp(item, InventoryManager.Instance.GetItemCount(item), uiManager, this);
        }
        itemListButtonDisplay.UpdateButtonGroup(); //update the button group to include the newly created buttons
    }
    private void ClearButtonContainer()
    {
        List<Transform> toDestroy = new List<Transform>();
        foreach (Transform child in buttonContainer)
        {
            if (child == sellItemButtonPrefab.transform || child == buyItemButtonPrefab.transform) continue;
            toDestroy.Add(child);
        }
        foreach (Transform child in toDestroy)
        {
            child.SetParent(null); // detaches immediately, unlike Destroy()
            Destroy(child.gameObject);
        }
    }
    public void EnterBuyMode()
    {
        PopulateBuyList();
        selectedItemButton = null;
        selectedItem = null;
        uiManager.HideItemDisplay();
        uiManager.ShowBuyModeButtons();
    }
    public void EnterSellMode()
    {
        PopulateSellList();
        selectedItemButton = null;
        selectedItem = null;
        uiManager.HideItemDisplay();
        uiManager.ShowSellModeButtons();
    }
    public void LeaveStore()
    {
        storeContent.SetActive(false);
        merchantStoreOpen = false;
        selectedItemButton = null;
    }
    public void EnterStore()
    {
        storeContent.SetActive(true);
        modeSwitchButtonDisplay.OnClick(buyModeSwitchButton); //select the buy mode button by default
        merchantStoreOpen = true;
        EnterBuyMode();
    }
}
