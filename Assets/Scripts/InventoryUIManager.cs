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

        Debug.Log(slot);
        Debug.Log(slot.gameItemSO.name);
        Debug.Log(slot.gameItemSO.inventorySprite);

        slotUI.SetIcon(slot.gameItemSO.inventorySprite);
        slotUI.SetAmount((int) slot.amount);
        slotUI.ShowChildren();
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

    
}
