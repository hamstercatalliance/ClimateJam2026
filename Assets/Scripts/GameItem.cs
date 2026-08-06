using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameItem : MonoBehaviour
{
    [SerializeField] private GameItemSO gameItemSO;
    [SerializeField] private string pickupID;
    public string PickupID 
    { 
        get { return pickupID; }
    }
    public GameItemSO GetGameItemSO()
    {
        return gameItemSO;
    }
    private void Start()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData && GameData.Instance.CollectedPickupIDs.Contains(PickupID))
        {
            Destroy(gameObject); // already picked up in a previous session
        }
    }
}
