using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColaQuest : Quest
{
    [SerializeField] private GameItemSO ColaSO;

    public int requiredColas { get; private set; } = 3;
    public bool CheckColas() {
        int items = InventoryManager.Instance.GetItemCount(ColaSO);
        if (requiredColas <= items) {
            return true;
        }
        return false;
    }

    public override void CompleteQuest()
    {
        base.CompleteQuest();
        InventoryManager.Instance.RemoveItemFromInventory(ColaSO, requiredColas);
    }
}
