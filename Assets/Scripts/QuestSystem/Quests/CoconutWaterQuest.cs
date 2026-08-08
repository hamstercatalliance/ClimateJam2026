using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutWaterQuest : Quest
{
    [SerializeField] GameItemSO CoconutWater;

    public bool CheckCocounutWater()
    {
        int items = InventoryManager.Instance.GetItemCount(CoconutWater);
        if (1 <= items)
        {
            return true;
        }
        return false;
    }
}
