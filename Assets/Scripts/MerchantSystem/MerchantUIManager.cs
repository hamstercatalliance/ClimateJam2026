using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MerchantUIManager : MonoBehaviour
{
    [SerializeField] private MerchantStore store;

    [SerializeField] private TextMeshProUGUI selectedItemNameText;
    [SerializeField] private TextMeshProUGUI selectedItemDescriptionText;
    [SerializeField] private GameObject selectedItemImage;

    [SerializeField] private GameObject buyModeButtons;
    [SerializeField] private GameObject sellModeButtons;
    private bool displayStayVisible = false;
    public bool GetDisplayStayVisible()
    {
        return displayStayVisible;
    }
    private void Start()
    {
        HideItemDisplay();
    }
    public void UpdateRemovedItemButton()
    {
        //in sell mode, we want to update the text on the button to display the remaining quantity avaible to be sold.
        //also, if it is sold out it should be greyed
        GameItemSO selectedItem = store.GetSelectedItem();
        if (selectedItem != null && store.GetSelectedItemButton() != null)
        {
            SellItemButtonUI selectedButton = store.GetSelectedItemButton().GetComponent<SellItemButtonUI>();
            if (selectedButton != null)
            {
                selectedButton.SetText(selectedItem, InventoryManager.Instance.GetItemCount(selectedItem));
                selectedButton.SetGreyedOut(InventoryManager.Instance.GetItemCount(selectedItem) <= 0);
            }
        }
    }
    public void SelectAndShowItemDisplay(GameItemSO itemSO)
    {
        ShowItemDisplay(itemSO);
        displayStayVisible = true;
        store.SetSelectedItem(itemSO);
    }
    public void ShowItemDisplay(GameItemSO itemSO)
    {
        //HOVER DISPLAY
        selectedItemNameText.text = itemSO.itemName;
        selectedItemDescriptionText.text = itemSO.itemDescription;
        selectedItemImage.GetComponent<Image>().sprite = itemSO.inventorySprite;
        selectedItemImage.SetActive(true);
    }
    public void HideItemDisplay()
    {
        displayStayVisible = false;
        selectedItemImage.SetActive(false);
        selectedItemNameText.text = "";
        selectedItemDescriptionText.text = "";
    }
    public void DeselectItemDisplay(GameItemSO itemSO)
    {
        displayStayVisible = false;
        ShowItemDisplay(itemSO); // mouse is prolly still hovering so keep it visible in hover mode
    }
    public void ShowBuyModeButtons()
    {
        buyModeButtons.SetActive(true);
        sellModeButtons.SetActive(false);
    }

    public void ShowSellModeButtons()
    {
        buyModeButtons.SetActive(false);
        sellModeButtons.SetActive(true);
    }
}
