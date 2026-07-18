using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class DiscardUIManager : MonoBehaviour
{
    public static DiscardUIManager Instance {get; private set;}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] GameObject discardUI; //will be hidden unless toggled on
    [SerializeField] TextMeshProUGUI text;
    private GameItemSO gameItem;
    private InventorySlotUI slot;
    public void ClearGameItem()
    {
        gameItem = null;
        slot = null;
    }
    private void Start()
    {
        InventorySlotUI.OnSlotClicked += OnSlotClicked;
    }
    private void Destroy()
    {
        InventorySlotUI.OnSlotClicked -= OnSlotClicked;
    }
    private void OnSlotClicked(object sender, InventorySlotUI.OnSlotClickedEventArgs e)
    {
        gameItem = e.item;
        slot = sender as InventorySlotUI;
    }
    public void OpenDiscardUI()
    {
        discardUI.SetActive(true);
        if (gameItem != null)
        {
            text.text = "Destroy " + InventoryManager.Instance.GetItemCount(gameItem) + " " + gameItem.itemName + "?";
        }
        else
        {
            Debug.LogError("Destroy panel shouldnt be opened with a null item");
            text.text = "ERROR";
        }
    }
    public void CloseDiscardUI()
    {
        discardUI.SetActive(false);
        text.text = "";
    }
    public void Discard()
    {
        InventoryManager.Instance.DiscardAllOfGameItem(gameItem);
        CloseDiscardUI();
        Debug.Log(slot);
        slot.ClearSlot();
        ItemDisplay.Instance.HideDisplay();
        ClearGameItem();
    }
}