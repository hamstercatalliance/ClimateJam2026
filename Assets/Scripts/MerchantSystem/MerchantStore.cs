using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MerchantStore : MonoBehaviour
{
    [Header("Modify these lists to change what items are available for purchase or sale.")]
    [SerializeField] private List<GameItemSO> sellableItems;
    [SerializeField] private List<GameItemSO> purchasableItems;
    [Header("Do not touch these unless you know what you're doing.")]
    [SerializeField] private MerchantUIManager uiManager;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buyItemButtonPrefab;    
    [SerializeField] private GameObject sellItemButtonPrefab;
    [SerializeField] private GameObject storeContent;
    [SerializeField] private OnClickButtonDisplay modeSwitchButtonDisplay;
    [SerializeField] private OnClickButtonDisplay itemListButtonDisplay;
    [SerializeField] private GameObject buyModeSwitchButton;
    public static event EventHandler OnTransaction;
    public static event EventHandler OnStoreEntered;
    public static event EventHandler OnStoreExited;
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
    private void Start()
    {
        Debug.Log("MerchantStore enabled, subscribing to currency change event.");
        CurrencyManager.Instance.OnCurrencyChanged += OnCurrencyChanged;
        EnterBuyMode(); // Default to buy mode when the store is opened
    }

    private void OnDestroy()
    {
        CurrencyManager.Instance.OnCurrencyChanged -= OnCurrencyChanged;
    }
    private void OnCurrencyChanged(object sender, CurrencyManager.OnCurrencyChangedEventArgs e)
    {
        Debug.Log("Currency changed, updating buy list.");
        if (currentMode == MerchantStoreMode.Buy)
        {
            PopulateBuyList();
        }
    }
    private enum MerchantStoreMode
    {
        Buy,
        Sell
    }
    private MerchantStoreMode currentMode = MerchantStoreMode.Buy;
    public void PurchaseItem()
    {
        if (selectedItem.cost > CurrencyManager.Instance.GetCurrency())
        {
            return; 
        }

        if (selectedItem.isVanityItem)
        {
            if (VanityManager.Instance.HasVanityItem(selectedItem))
            {
                return;
            }
            VanityManager.Instance.AddVanityItem(selectedItem);
        }
        else
        {
            InventoryManager.Instance.AddItemToInventory(selectedItem);
        }

        CurrencyManager.Instance.RemoveCurrency(selectedItem.cost);
        //Debug.Log("Purchased item: " + selectedItem.name);
        OnTransaction?.Invoke(this, EventArgs.Empty);
    }
    public void SellItem()
    {
        //remove item from inventory and add currency to player
        InventoryManager.Instance.RemoveItemFromInventory(selectedItem);
        CurrencyManager.Instance.AddCurrency(selectedItem.cost);
        uiManager.UpdateRemovedItemButton();
        //Debug.Log("Sold item: " + selectedItem.name);
        OnTransaction?.Invoke(this, EventArgs.Empty);
    }
    public void DonateItem()
    {
        //remove item from inventory and add SYMPATHY POINTS to player
        InventoryManager.Instance.RemoveItemFromInventory(selectedItem);
        uiManager.UpdateRemovedItemButton();
        //Debug.Log("Donated item: " + selectedItem.name);
        OnTransaction?.Invoke(this, EventArgs.Empty);
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
        currentMode = MerchantStoreMode.Buy;
    }
    public void EnterSellMode()
    {
        PopulateSellList();
        selectedItemButton = null;
        selectedItem = null;
        uiManager.HideItemDisplay();
        uiManager.ShowSellModeButtons();
        currentMode = MerchantStoreMode.Sell;
    }
    public void LeaveStore()
    {
        storeContent.SetActive(false);
        merchantStoreOpen = false;
        selectedItemButton = null;

        OnStoreExited?.Invoke(this, EventArgs.Empty);
    }
    public void EnterStore()
    {
        storeContent.SetActive(true);
        modeSwitchButtonDisplay.OnClick(buyModeSwitchButton); //select the buy mode button by default
        merchantStoreOpen = true;
        Debug.Log("MerchantStore enabled, entering store and subscribing to currency change event.");
        EnterBuyMode();

        OnStoreEntered?.Invoke(this, EventArgs.Empty);
    }
}
