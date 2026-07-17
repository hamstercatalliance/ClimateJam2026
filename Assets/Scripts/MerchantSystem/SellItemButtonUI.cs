using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class SellItemButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Button sellButton;
    private GameItemSO gameItemSO;
    private int quantityOwned;
    [SerializeField] private MerchantUIManager uiManager;
    [SerializeField] private MerchantStore store;
    public void SetText(GameItemSO item, int quantity)
    {
        itemText.text = item.name + "(x" + quantity + ") - $" + item.cost.ToString();
    }
    public void SetUp(GameItemSO item, int quantity)
    {
        SetText(item, quantity);
        gameItemSO = item;
        quantityOwned = quantity;

        bool canSell = quantity > 0;
        sellButton.interactable = canSell;

        Color c = itemText.color;
        c.a = canSell ? 1f : 0.4f;
        itemText.color = c;

        sellButton.onClick.RemoveAllListeners();
        if (canSell)
        {
            sellButton.onClick.AddListener(OnSelectButtonPressed);
        }
    }

    private void OnSelectButtonPressed()
    {
        if (store.GetSelectedItemButton() == gameObject)
        {
            uiManager.DeselectItemDisplay(gameItemSO);
            store.SetSelectedItemButton(null);
            store.SetSelectedItem(null);
        }
        else
        {
            uiManager.SelectAndShowItemDisplay(gameItemSO);
            store.SetSelectedItemButton(gameObject);
        }
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (uiManager.GetDisplayStayVisible() == false)
        {
            uiManager.ShowItemDisplay(gameItemSO);
        }
        //hover
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!uiManager.GetDisplayStayVisible())
        {
            uiManager.HideItemDisplay();
        }
        //hover exit
    }
}
