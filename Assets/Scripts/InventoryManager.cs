using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; } //singleton

    [SerializeField] private List<GameItemSO> gameItemSOList; 
    //all possible storable items in the game
    //depending on whether or not we make storable vs non storable items this may come in handy 
    //but it has not been implemented yet
    public struct InventorySlot
    {
        public GameItemSO gameItemSO;
        public int? amount;
        public bool isOccupied;
        public InventorySlot(GameItemSO gameItemSO, int? amount, bool isOccupied)
        {
            this.gameItemSO = gameItemSO;
            this.amount = amount;
            this.isOccupied = isOccupied;
        }
    }
    private InventorySlot[,] inventorySlots = new InventorySlot[3, 4];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventorySlots.GetLength(0); i++)
        {
            for (int j = 0; j < inventorySlots.GetLength(1); j++)
            {
                inventorySlots[i, j] = new InventorySlot(null, null, false);
            }
        }
        Player.Instance.OnPickup += Player_OnPickup;
    }
    private void Player_OnPickup(object sender, Player.OnPickupEventArgs e)
    {
        Debug.Log("Player stored " + e.gameItemSO.name);
        AddItemToInventory(e.gameItemSO);
        Destroy(e.gameItemGameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddItemToInventory(GameItemSO gameItemSO)
    {
        InventorySlot? existingItem = InventoryContainsItem(gameItemSO);
        if (existingItem != null)
        {
            //player already has this item in their inventory
            InventorySlot item = existingItem.Value;
            item.amount += 1;
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
}
