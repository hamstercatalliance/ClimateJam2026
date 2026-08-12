using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingCarp : Quest
{
    public GameItemSO carp;
    public GameItemSO sturgeon;
    public GameItemSO catfish;

    [SerializeField] private GameItemSO fishingRod;
    [SerializeField] private GameItemSO bait;
    [SerializeField] private GameObject playerFishingRod;
    private int carpRequirement = 5;

    protected override void Start()
    {
        base.Start();
        playerFishingRod.SetActive(false);
    }
    public bool CheckCarp()
    {
        //CALL THIS AFTER EVERY FISHING ATTEMPT TO CHECK IF THE PLAYER HAS ENOUGH CARP TO COMPLETE THE QUEST
        int items = InventoryManager.Instance.GetItemCount(carp);
        if (carpRequirement <= items)
        {
            base.CompleteQuest();
            return true;
        }
        Debug.Log("FishingCarp: Carp count insufficient");
        return false;
    }

    public bool HasFishingRod()
    {
        int items = InventoryManager.Instance.GetItemCount(fishingRod);
        Debug.Log($"FishingCarp: Fishing rod count: {items}");
        if (items > 0)
        {
            Debug.Log("FishingCarp: Fishing rod available");
            return true;
        }
        Debug.Log("FishingCarp: Fishing rod not available");
        return false;
    }

    public bool HasBait()
    {
        int items = InventoryManager.Instance.GetItemCount(bait);
        Debug.Log($"FishingCarp: Bait count: {items}");
        if (items > 0)
        {
            Debug.Log("FishingCarp: Bait available");
            return true;
        }
        Debug.Log("FishingCarp: Bait not available");
        return false;
    }

    public GameItemSO Fish()
    {
        InventoryManager.Instance.RemoveItemFromInventory(bait, 1);
        int randomNumber = Random.Range(0, 3); 
        if (randomNumber == 0)
        {
            return carp;
        }
        else if (randomNumber == 1)
        {
            return sturgeon;
        }
        else
        {
            return catfish;
        }
    }

    public void ReelFish(GameItemSO gameItemSO)
    {
        Debug.Log($"FishingCarp: ReelFish called with itemID: {gameItemSO.itemID}");

        if (gameItemSO.itemID == "item.carp")
        {
            InventoryManager.Instance.AddItemToInventory(carp);
            SympathyPointsManager.Instance.addSympathyPoints(25);

            Debug.Log(InventoryManager.Instance.GetItemCount(carp));

            CheckCarp();
        }
        else if (gameItemSO.itemID == "item.sturgeon")
        {
            InventoryManager.Instance.AddItemToInventory(sturgeon);
            SympathyPointsManager.Instance.addSympathyPoints(-50);
        }
        else if (gameItemSO.itemID == "item.catfish")
        {
            InventoryManager.Instance.AddItemToInventory(catfish);
            //No sympathy points for catfish
        }
    }

    public void ShowFishingRod()
    {
        playerFishingRod.SetActive(true);
    }
    public void HideFishingRod()
    {
        playerFishingRod.SetActive(false);
    }
}
