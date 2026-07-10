using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public bool HasLoadedRunData { get; set; }

    public InventoryManager.InventorySlot[,] Inventory { get; set; }

    public float DayManagerTimeElapsed { get; set; }
    public int DayManagerDayCount { get; set; }
    
    public float? DayManagerUITransitionProgress { get; set; }

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}