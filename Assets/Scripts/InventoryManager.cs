using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InventoryManager : MonoBehaviour, IHasPersistentData
{
    public static InventoryManager Instance { get; private set; } //singleton
    [SerializeField] private List<GameItemSO> gameItemSOList; 
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
    //all possible storable items in the game
    //depending on whether or not we make storable vs non storable items this may come in handy 
    //but it has not been implemented yet
    public struct InventorySlot
    {
        public GameItemSO gameItemSO;
        public int? amount;
        public bool isOccupied;
        public int row;
        public int col;
        public InventorySlot(GameItemSO gameItemSO, int? amount, bool isOccupied, int row, int col)
        {
            this.gameItemSO = gameItemSO;
            this.amount = amount;
            this.isOccupied = isOccupied;
            this.row = row;
            this.col = col;
        }
    }
    private InventorySlot[,] inventorySlots = new InventorySlot[3, 4];
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
        Destroy(e.gameItemGameObject);
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            inventorySlots = GameData.Instance.InventorySlots;
            InventoryUIManager.Instance.LoadSlotUIData();
        }
        else
        {
            //new game
            for (int i = 0; i < inventorySlots.GetLength(0); i++)
            {
                for (int j = 0; j < inventorySlots.GetLength(1); j++)
                {
                    inventorySlots[i, j] = new InventorySlot(null, null, false, i, j);
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

            OnInventoryAddition?.Invoke(this, new InventoryAdditionEventArgs
            {
                slot = slot
            });
            Debug.Log("Player stored an additional" + slot.gameItemSO.name);

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
                        inventorySlots[i, j].gameItemSO = gameItemSO;
                        inventorySlots[i, j].row = i;
                        inventorySlots[i, j].col = j;

                        OnInventoryAddition?.Invoke(this, new InventoryAdditionEventArgs
                        {
                            slot = inventorySlots[i, j]
                        });
                        Debug.Log("Player stored " + inventorySlots[i, j].gameItemSO.name + "at" + i + "," + j);
                        
                        return true;
                    }
                }
            }
        }
        //inventory is full
        return false;
    }
    public bool RemoveItemFromInventory(GameItemSO gameItemSO)
    {
        InventorySlot? existingItem = InventoryContainsItem(gameItemSO);
        if (existingItem != null)
        {
            InventorySlot item = existingItem.Value;
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
                item.gameItemSO = null;
                item.amount = null;
            }
            return true;
        }
        return false;
    }
    public InventorySlot? InventoryContainsItem(GameItemSO gameItemSO)
    {
        for (int i = 0; i < inventorySlots.GetLength(0); i++)
        {
            for (int j = 0; j < inventorySlots.GetLength(1); j++)
            {
                if (inventorySlots[i, j].isOccupied && inventorySlots[i, j].gameItemSO == gameItemSO)
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
}
