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
    [SerializeField] private CanvasGroup canvasGroup;
    private GameItemSO gameItemSO;
    private MerchantUIManager uiManager;
    private MerchantStore store;
    public void SetText(GameItemSO item, int quantity)
    {
        itemText.text = item.itemName + "(x" + quantity + ") - $" + item.cost.ToString();
    }
    public void SetUp(GameItemSO item, int quantity, MerchantUIManager uiManager, MerchantStore store)
    {
        this.uiManager = uiManager;
        this.store = store;
        SetText(item, quantity);
        gameItemSO = item;

        bool canSell = quantity > 0;
        sellButton.interactable = canSell;

        sellButton.onClick.RemoveAllListeners();
        if (canSell)
        {
            sellButton.onClick.AddListener(OnSelectButtonPressed);
        }
        SetGreyedOut(!canSell);
    }
    public void SetGreyedOut(bool greyedOut)
    {
        canvasGroup.alpha = greyedOut ? 0.4f : 1f;
        canvasGroup.interactable = !greyedOut;
        canvasGroup.blocksRaycasts = !greyedOut; //stops hover/click events from firing at all
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
