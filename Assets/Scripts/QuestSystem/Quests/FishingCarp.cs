using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingCarp : Quest
{
    [SerializeField] private GameItemSO carp;
    [SerializeField] private GameItemSO fishingRod;
    [SerializeField] private GameItemSO bait;
    private int carpRequirement = 5;

    public bool CheckCarp()
    {
        int items = InventoryManager.Instance.GetItemCount(carp);
        if (carpRequirement <= items)
        {
            Debug.Log("FishingCarp: Carp count sufficient");
            return true;
        }
        Debug.Log("FishingCarp: Carp count insufficient");
        return false;
    }

    public bool CheckFishingRod()
    {
        int items = InventoryManager.Instance.GetItemCount(fishingRod);
        if (items > 0)
        {
            Debug.Log("FishingCarp: Fishing rod available");
            return true;
        }
        Debug.Log("FishingCarp: Fishing rod not available");
        return false;
    }

    public bool CheckBait()
    {
        int items = InventoryManager.Instance.GetItemCount(bait);
        if (items > 0)
        {
            Debug.Log("FishingCarp: Bait available");
            return true;
        }
        Debug.Log("FishingCarp: Bait not available");
        return false;
    }
}
