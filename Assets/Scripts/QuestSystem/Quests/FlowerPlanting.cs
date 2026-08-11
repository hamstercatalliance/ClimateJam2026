using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlanting : Quest
{
    [SerializeField] private GameItemSO flowerSeeds;

    public bool CheckFlowerSeeds()
    {
        int items = InventoryManager.Instance.GetItemCount(flowerSeeds);
        if (1 <= items)
        {
            Debug.Log("PlantingFlowers: Flower seeds count sufficient");
            return true;
        }

        Debug.Log("PlantingFlowers: Flower seeds count insufficient");
        return false;
    }
}
