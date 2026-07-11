using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    private Vector3 originalScale;
    private void Start()
    {
        originalScale = icon.transform.localScale;
    }
    private GameItemSO item;
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI amountText;
    public static event EventHandler<OnSlotHoveredEventArgs> OnSlotHovered;
    public class OnSlotHoveredEventArgs : EventArgs
    {
        public GameItemSO item;
    }
    public static event EventHandler OnSlotHoverExit;
    public static event EventHandler OnSlotClicked;
    public void SetIcon(GameItemSO gameItemSO)
    {
        Debug.Log("Setting icon for slot");
        item = gameItemSO;
        icon.GetComponent<Image>().sprite = gameItemSO.inventorySprite;
        icon.SetActive(true); //EVENTUALLY HANDLE THIS ELSEWHERE
    }
    public void ClearSlot()
    {
        //clear the slot
        icon.SetActive(false); //EVENTUALLY HANDLE THIS ELSEWHERE
        amountText.text = "";
    }
    public void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
        Debug.Log("Setting amount for slot");
    }
    public void ShowChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse over slot");
        if (item != null)
        {
            icon.transform.localScale = originalScale * hoverScale;
            OnSlotHovered?.Invoke(this, new InventorySlotUI.OnSlotHoveredEventArgs 
            { 
                item = item 
            });
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited slot");
        icon.transform.localScale = originalScale;
        OnSlotHoverExit?.Invoke(this, EventArgs.Empty);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(this, EventArgs.Empty);
    }
}
