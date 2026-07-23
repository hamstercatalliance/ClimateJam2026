using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class ItemDisplay : MonoBehaviour
{
    public static ItemDisplay Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private GameObject itemImageObject;
    [SerializeField] private GameObject textObjects;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private GameObject learnMoreObject;
    [SerializeField] private GameObject trashButton;
    private bool stayVisible = false;
    public void SetStayVisible(bool value)
    {
        stayVisible = value;
    }
    private string sourceLink;
    private void OnEnable()
    {
        HideDisplay();
    }
    void Start()
    {
        HideDisplay();
        InventorySlotUI.OnSlotHovered += OnSlotHovered;
        InventorySlotUI.OnSlotHoverExit += OnSlotHoverExit;
        InventorySlotUI.OnSlotClicked += OnSlotClicked;
        LinkOpener.OnLinkClicked += OnLinkClicked;
    }
    private void OnDestroy()
    {
        InventorySlotUI.OnSlotHovered -= OnSlotHovered;
        InventorySlotUI.OnSlotHoverExit -= OnSlotHoverExit;
        InventorySlotUI.OnSlotClicked -= OnSlotClicked;
        LinkOpener.OnLinkClicked -= OnLinkClicked;
    }
    private void OnLinkClicked(object sender, EventArgs e)
    {
        if (sourceLink != null)
        {
            LinkOpener.OpenLink(sourceLink);
        }
    }
    private void OnSlotHovered(object sender, InventorySlotUI.OnSlotHoveredEventArgs e)
    {
        GameItemSO item = e.item;
        if (stayVisible == false)
        {
            ShowDisplay(item);
        }
    }

    private void OnSlotHoverExit(object sender, EventArgs e)
    {
        if (stayVisible == false)
        {
            HideDisplay();
        }
    }
    private GameItemSO lockedItem;
    private void OnSlotClicked(object sender, InventorySlotUI.OnSlotClickedEventArgs e)
    {
        if (stayVisible && lockedItem == e.item)
        {
            stayVisible = false;
            lockedItem = null;
            HideDisplay();
        }
        else
        {
            stayVisible = true;
            lockedItem = e.item;
            ShowDisplay(e.item);
        }
    }
    public void HideDisplay()
    {
        itemImageObject.SetActive(false);
        textObjects.SetActive(false);
        trashButton.SetActive(false);

        itemNameText.text = "";
        itemDescriptionText.text = "";
        learnMoreObject.GetComponent<TextMeshProUGUI>().text = "";
        sourceLink = null;
    }
    private void ShowDisplay(GameItemSO gameItemSO)
    {
        itemImageObject.GetComponent<Image>().sprite = gameItemSO.inventorySprite;
        itemNameText.text = gameItemSO.itemName;
        itemDescriptionText.text = gameItemSO.itemDescription;
        sourceLink = gameItemSO.sourceLink;
        if (sourceLink == null || sourceLink == "")
        {
            learnMoreObject.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            learnMoreObject.GetComponent<TextMeshProUGUI>().text = "Learn More";
        }
        itemImageObject.SetActive(true);
        textObjects.SetActive(true);
        trashButton.SetActive(true);
    }
}
