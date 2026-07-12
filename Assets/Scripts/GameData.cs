using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public bool HasLoadedRunData = false;

    public InventoryManager.InventorySlot[,] InventorySlots;

    public float DayManagerTimeElapsed;
    public int DayManagerDayCount;

    public int? SympathyPoints;

    public float? DayManagerUITransitionProgress;

    public Vector3 PlayerFacingDirection;
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