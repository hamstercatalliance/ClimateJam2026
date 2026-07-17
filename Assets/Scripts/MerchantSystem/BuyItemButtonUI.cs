using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BuyItemButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Button buyButton;
    private GameItemSO gameItemSO;
    [SerializeField] private MerchantUIManager uiManager;
    [SerializeField] private MerchantStore store;
    
    public void SetUp(GameItemSO item)
    {
        gameItemSO = item;
        itemText.text = gameItemSO.name + " - $" + gameItemSO.cost.ToString();
        
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(OnSelectButtonPressed);
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
