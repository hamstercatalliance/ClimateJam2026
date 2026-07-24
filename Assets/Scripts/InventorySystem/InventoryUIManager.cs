using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }
    [SerializeField] private InventorySlotUI[] row0;
    [SerializeField] private InventorySlotUI[] row1;
    [SerializeField] private InventorySlotUI[] row2;
    [SerializeField] private InventorySlotUI[] row3;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.Instance.OnInventoryAddition += InventoryManager_OnInventoryAddition;
        InventoryManager.Instance.OnInventoryRemoval += InventoryManager_OnInventoryRemoval;
    }
    
    private void InventoryManager_OnInventoryAddition(object sender, InventoryManager.InventoryAdditionEventArgs e)
    {
        Debug.Log("Inventory addition event triggered");
        InventoryManager.InventorySlot slot = e.slot;
        InventorySlotUI slotUI = GetSlotUI(slot.row, slot.col);

        ShowSlot(slotUI, slot);
    }
    private void InventoryManager_OnInventoryRemoval(object sender, InventoryManager.InventoryRemovalEventArgs e)
    {
        Debug.Log("Inventory removal event triggered");
        InventorySlotUI slotUI = GetSlotUI(e.row, e.col);
        slotUI.ClearSlot();
    }

    public InventorySlotUI GetSlotUI(int row, int col)
    {
        switch (row)
        {
            case 0:
                return row0[col];
            case 1:
                return row1[col];
            case 2:
                return row2[col];
            case 3:
                return row3[col];
            default:
                throw new ArgumentOutOfRangeException(nameof(row), row, null);
        }
    }

    public void LoadSlotUIData()
    {
        for (int row = 0; row < GameData.Instance.InventorySlots.GetLength(0); row++)
        {
            for (int col = 0; col < GameData.Instance.InventorySlots.GetLength(1); col++)
            {
                InventoryManager.InventorySlot slot = GameData.Instance.InventorySlots[row, col];
                InventorySlotUI slotUI = GetSlotUI(row, col);
                
                if (slot.isOccupied)
                {
                    ShowSlot(slotUI, slot);
                }
            }
        }
    }
    private void ShowSlot(InventorySlotUI slotUI, InventoryManager.InventorySlot slot)
    {
        GameItemSO item = ScriptableObjectDatabase.Instance.GetScriptableObjectByID(slot.itemID) as GameItemSO;
        slotUI.SetIcon(item);
        slotUI.SetAmount(slot.amount);
        slotUI.ShowChildren();
    }
}
