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
    public float DayManagerUITransitionProgress;
    #endregion
    #region Player data
    public Vector3 PlayerFacingDirection;
    #endregion

    #region Sympathy Points Data
    public int SympathyPoints; 
    #endregion

    #region Quest Manager Data (DOES NOT SAVE QUEST SPECIFIC PROGRESS. ONLY SAVES: questSO, isCompleted, isInitiated)
    public List<QuestManager.QuestData> QuestDataList = new List<QuestManager.QuestData>();
    #endregion
    #region Custom Quest Data (SAVE SPECIFIC PROGRESS FOR CUSTOM QUESTS.)
    public int TestQuestProgress;
    #endregion
    #region Currency Manager Data
    public int currencyAmount;
    #endregion
    

    [Serializable]
    public class SaveData
    {
        public List<InventoryManager.InventorySlot> InventorySlotsFlat; //flat, not [,]
        public int InventoryRows;
        public int InventoryCols;
        public float DayManagerTimeElapsed;
        public int DayManagerDayCount;
        public float DayManagerUITransitionProgress;
        public Vector3 PlayerFacingDirection;
        public int SympathyPoints;
        public List<QuestManager.QuestData> QuestDataList = new List<QuestManager.QuestData>();
        public int currencyAmount;
    }
    public SaveData GetSaveData()
    {
        SaveData saveData = new SaveData();
        if (InventorySlots != null)
        {
            saveData.InventoryRows = InventorySlots.GetLength(0);
            saveData.InventoryCols = InventorySlots.GetLength(1);
            saveData.InventorySlotsFlat = new List<InventoryManager.InventorySlot>();
            for (int row = 0; row < saveData.InventoryRows; row++)
            {
                for (int col = 0; col < saveData.InventoryCols; col++)
                {
                    saveData.InventorySlotsFlat.Add(InventorySlots[row, col]);
                }
            }
        }
        saveData.DayManagerTimeElapsed = DayManagerTimeElapsed;
        saveData.DayManagerDayCount = DayManagerDayCount;
        saveData.DayManagerUITransitionProgress = 0; //loading data should always be at the start of the day
        saveData.PlayerFacingDirection = PlayerFacingDirection;
        saveData.SympathyPoints = SympathyPoints;
        saveData.QuestDataList = QuestDataList;
        saveData.currencyAmount = currencyAmount;

        return saveData;
    }
    private void SetSaveData(SaveData data)
    {
        UnflattenInventorySlots(data);
        DayManagerTimeElapsed = data.DayManagerTimeElapsed;
        DayManagerDayCount = data.DayManagerDayCount;
        DayManagerUITransitionProgress = data.DayManagerUITransitionProgress;
        PlayerFacingDirection = data.PlayerFacingDirection;
        SympathyPoints = data.SympathyPoints;
        QuestDataList = data.QuestDataList;
        currencyAmount = data.currencyAmount;
    }
    public void LoadFromSaveData(SaveData data)
    {
        SetSaveData(data);
        HasLoadedRunData = true;
    }
    public void ClearData()
    {
        InventorySlots = null;
        DayManagerTimeElapsed = 0f;
        DayManagerDayCount = 1;
        DayManagerUITransitionProgress = 0;
        PlayerFacingDirection = Vector3.zero;
        SympathyPoints = 0;
        QuestDataList.Clear();
        currencyAmount = 0;
        HasLoadedRunData = false;
    }



    private void UnflattenInventorySlots(SaveData data)
    {
        if (data.InventorySlotsFlat != null && data.InventoryRows > 0 && data.InventoryCols > 0)
        {
            InventorySlots = new InventoryManager.InventorySlot[data.InventoryRows, data.InventoryCols];
            int i = 0;
            for (int row = 0; row < data.InventoryRows; row++)
            {
                for (int col = 0; col < data.InventoryCols; col++)
                {
                    InventorySlots[row, col] = data.InventorySlotsFlat[i];
                    i++;
                }
            }
        }
    }
}