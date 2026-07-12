using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    #region Singleton and personal data
    public static GameData Instance { get; private set; }
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
    public bool HasLoadedRunData = false;
    #endregion
    #region Inventory Data
    public InventoryManager.InventorySlot[,] InventorySlots;
    #endregion
    #region Day Manager Data
    public float DayManagerTimeElapsed;
    public int DayManagerDayCount;
    #endregion
    #region Day Manager UI Data
    public float? DayManagerUITransitionProgress;
    #endregion
    #region Player data
    public Vector3 PlayerFacingDirection;
    #endregion
    #region Quest Manager Data (DOES NOT SAVE QUEST SPECIFIC PROGRESS. ONLY SAVES: questSO, isCompleted, isInitiated)
    public List<QuestManager.QuestData> QuestDataList = new List<QuestManager.QuestData>();
    #endregion
    #region Currency Manager Data
    public int currencyAmount;
    #endregion
    public int TestQuestProgress;
}