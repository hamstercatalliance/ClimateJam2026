using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class VanitySlotUI : SlotUI
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private GameObject icon;
    //[SerializeField] private Sprite[] slotBackgroundSprites;
    private Vector3 iconOriginalScale;
    private GameItemSO vanityItemSO;
    private void Start()
    {
        iconOriginalScale = icon.transform.localScale;
        //GetComponent<Image>().sprite = slotBackgroundSprites[UnityEngine.Random.Range(0, slotBackgroundSprites.Length)];
    }
    public static event EventHandler<OnSlotHoveredEventArgs> OnVanitySlotHovered;
    public class OnSlotHoveredEventArgs : EventArgs
    {
        public GameItemSO vanityItemSO;
    }
    public static event EventHandler OnVanitySlotHoverExit;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        icon.transform.localScale = iconOriginalScale * hoverScale;
        OnVanitySlotHovered?.Invoke(this, new OnSlotHoveredEventArgs 
        { 
            vanityItemSO = vanityItemSO 
        });
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        icon.transform.localScale = iconOriginalScale;
        OnVanitySlotHoverExit?.Invoke(this, EventArgs.Empty);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (VanityManager.Instance.GetSelectedVanityItem() == null)
        {
            //nothing is selected, so select this item
            VanityManager.Instance.SetSelectedVanityItem(vanityItemSO);
            VanityPlayerPreview.Instance.SetStayVisible(true);
        }
        else if (VanityManager.Instance.GetSelectedVanityItem() == vanityItemSO)
        {
            //this item is already selected, so deselect it
            VanityManager.Instance.ClearSelectedVanityItem();
            VanityPlayerPreview.Instance.SetStayVisible(false);
        }
        else
        {
            //another item is selected, so select this item instead
            VanityManager.Instance.SetSelectedVanityItem(vanityItemSO);
            VanityPlayerPreview.Instance.SetStayVisible(true);
        }
        icon.transform.localScale = iconOriginalScale;
    }
    public void SetItem(GameItemSO gameItemSO)
    {
        vanityItemSO = gameItemSO;
        icon.GetComponent<Image>().sprite = gameItemSO.inventorySprite;
        icon.SetActive(true);
    }
}
