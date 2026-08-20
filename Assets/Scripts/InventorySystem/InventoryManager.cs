using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InventoryManager : MonoBehaviour, IHasPersistentData
{
    public static InventoryManager Instance { get; private set; } //singleton
    public event EventHandler OnItemDiscarded;
    public event EventHandler<InventoryAdditionEventArgs> OnInventoryAddition;
    public class InventoryAdditionEventArgs : EventArgs
    {
        public InventorySlot slot;
    }
    public event EventHandler<InventoryRemovalEventArgs> OnInventoryRemoval;
    public class InventoryRemovalEventArgs : EventArgs
    {
        public int row;
        public int col;
    }
    public bool DataSuccessfullyWritten { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [Serializable]
    public struct InventorySlot
    {
        //public GameItemSO gameItemSO;
        public string itemID; //store the itemID instead of the GameItemSO reference
        public int amount;
        public bool isOccupied;
        public int row;
        public int col;
        public InventorySlot(GameItemSO gameItemSO, int amount, bool isOccupied, int row, int col)
        {
            //this.gameItemSO = gameItemSO;
            if (gameItemSO != null)
            {
                this.itemID = gameItemSO.itemID;
            }
            else
            {
                this.itemID = null;
            }
            this.amount = amount;
            this.isOccupied = isOccupied;
            this.row = row;
            this.col = col;
        }
    }
    private InventorySlot[,] inventorySlots = new InventorySlot[4, 3];
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.OnPickup += Player_OnPickup;
        SceneLoader.OnSceneTransition += SceneLoader_OnSceneTransition;
        LoadGameData();
    }
    private void SceneLoader_OnSceneTransition(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    private void Player_OnPickup(object sender, Player.OnPickupEventArgs e)
    {
        AddItemToInventory(e.gameItemSO);
        GameItem gameItem = e.gameItemGameObject.GetComponent<GameItem>();
        if (gameItem != null && !string.IsNullOrEmpty(gameItem.PickupID))
        {
            GameData.Instance.CollectedPickupIDs.Add(gameItem.PickupID);
        }
        Destroy(e.gameItemGameObject);
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            inventorySlots = GameData.Instance.InventorySlots;
            InventoryUIManager.Instance.LoadSlotUIData();
            //Debug.Log(ScriptableObjectDatabase.Instance.GetScriptableObjectByID(inventorySlots[0,0].itemID));
        }
        else
        {
            //new game
            for (int i = 0; i < inventorySlots.GetLength(0); i++)
            {
                for (int j = 0; j < inventorySlots.GetLength(1); j++)
                {
                    inventorySlots[i, j] = new InventorySlot(null, 0, false, i, j);
                }
            }
        }
        
    }

    public void WriteToGameData()
    {
        GameData.Instance.InventorySlots = inventorySlots;
        DataSuccessfullyWritten = true;
    }

    public bool AddItemToInventory(GameItemSO gameItemSO)
    {
        InventorySlot? existingItem = InventoryContainsItem(gameItemSO);
        if (existingItem != null)
        {
            //player already has this item in their inventory
            InventorySlot slot = existingItem.Value;
            slot.amount += 1;
            inventorySlots[slot.row, slot.col] = slot;
            
            OnInventoryAddition?.Invoke(this, new InventoryAdditionEventArgs
            {
                slot = slot
            });
            //Debug.Log("Player stored an additional" + slot.gameItemSO.name);
            //Debug.Log("Player now has " + slot.amount + " " + slot.gameItemSO.name);
            return true;
        }
        else
        {
            //find the first empty slot and add the item to it
            for (int i = 0; i < inventorySlots.GetLength(0); i++)
            {
                for (int j = 0; j < inventorySlots.GetLength(1); j++)
                {
                    if (!inventorySlots[i, j].isOccupied)
                    {
                        inventorySlots[i, j].amount = 1;
                        inventorySlots[i, j].isOccupied = true;
                        //inventorySlots[i, j].gameItemSO = gameItemSO;
                        inventorySlots[i, j].itemID = gameItemSO.itemID;
                        inventorySlots[i, j].row = i;
                        inventorySlots[i, j].col = j;

                        OnInventoryAddition?.Invoke(this, new InventoryAdditionEventArgs
                        {
                            slot = inventorySlots[i, j]
                        });
                        
                        return true;
                    }
                }
            }
        }
        //inventory is full
        return false;
    }
    public bool RemoveItemFromInventory(GameItemSO gameItemSO, int count = 1)
    {
        InventorySlot? existingItem = InventoryContainsItem(gameItemSO);
        if (existingItem != null)
        {

            InventorySlot item = existingItem.Value;
            for (int i = 0; i < count; i++)
            {
                item.amount -= 1;
                if (item.amount <= 0)
                {
                    OnInventoryRemoval?.Invoke(this, new InventoryRemovalEventArgs
                    {
                        row = item.row,
                        col = item.col
                    });
                    //empty the slot
                    item.isOccupied = false;
                    //item.gameItemSO = null;
                    item.itemID = null;
                    item.amount = 0;
                }
                inventorySlots[item.row, item.col] = item;
            }
            if (item.amount > 0) //update the UI if there are still items left in the slot
            {
                OnInventoryAddition?.Invoke(this, new InventoryAdditionEventArgs
                {
                    slot = item
                });
            }
            else
            {
                OnInventoryRemoval?.Invoke(this, new InventoryRemovalEventArgs
                {
                    row = item.row,
                    col = item.col
                });
            }
            return true;
        }
        return false;
    }
    public InventorySlot? InventoryContainsItem(GameItemSO gameItemSO)
    {
        string itemID = gameItemSO.itemID;
        for (int i = 0; i < inventorySlots.GetLength(0); i++)
        {
            for (int j = 0; j < inventorySlots.GetLength(1); j++)
            {
                if (inventorySlots[i, j].isOccupied && inventorySlots[i, j].itemID == itemID)
                {
                    return inventorySlots[i, j];
                }
            }
        }
        return null;
    }
    public bool ItemExistsAtSlot(int row, int col)
    {
        return inventorySlots[row, col].isOccupied;
    }

    public int GetItemCount(GameItemSO gameItemSO)
    {
        InventorySlot? slot = InventoryContainsItem(gameItemSO);
        return slot?.amount ?? 0;
    }

    public void DiscardAllOfGameItem(GameItemSO gameItemSO)
    {
        InventorySlot? existingItem = InventoryContainsItem(gameItemSO);
        if (existingItem != null)
        {
            InventorySlot item = existingItem.Value;
            OnInventoryRemoval?.Invoke(this, new InventoryRemovalEventArgs
            {
                row = item.row,
                col = item.col
            });
            //empty the slot
            item.isOccupied = false;
            //item.gameItemSO = null;
            item.itemID = null;
            item.amount = 0;
            inventorySlots[item.row, item.col] = item;

            OnItemDiscarded?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsFull()
    {
        for (int i = 0; i < inventorySlots.GetLength(0); i++)
        {
            for (int j = 0; j < inventorySlots.GetLength(1); j++)
            {
                if (!inventorySlots[i, j].isOccupied)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
