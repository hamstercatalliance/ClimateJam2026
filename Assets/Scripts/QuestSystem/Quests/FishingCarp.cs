using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingCarp : Quest
{
    [SerializeField] private GameItemSO carp;
    private int carpRequirement = 5;

    public bool CheckFlowerSeeds()
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
}
