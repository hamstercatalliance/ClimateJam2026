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
    public bool HasCompletedFirstDay;
    public bool IsStartOfNewDay = true;
    #endregion
    #region Player data
    public Vector3 PlayerFacingDirection;
    public Vector3 PlayerSpawnPosition;
    public bool HasPendingSpawnPosition;
    #endregion
    #region Sympathy Points Data
    public int SympathyPoints; 
    #endregion

    #region Quest Manager Data (DOES NOT SAVE QUEST SPECIFIC PROGRESS. ONLY SAVES: questSO, isCompleted, isInitiated)
    public List<QuestManager.QuestData> QuestDataList = new List<QuestManager.QuestData>();
    public bool HasQuestNotificationActive;
    #endregion
    #region Custom Quest Data (SAVE SPECIFIC PROGRESS FOR CUSTOM QUESTS.)
    public int TestQuestProgress;
    public bool OpenedQuestBoard;
    public int BeachCleanupProgress;
    #endregion
    #region Currency Manager Data
    public int currencyAmount;
    #endregion
    #region Vanity Items Data
    public List<string> OwnedVanityItemIDs = new List<string>();
    public string EquippedVanityItemID;
    #endregion
    #region Sound and Music Data
    public float SoundVolume;
    public float MusicVolume;
    #endregion
    #region Pickup Tracking Data
    public List<string> CollectedPickupIDs = new List<string>();
    #endregion
    [Serializable]
    public class SaveData
    {
        public List<InventoryManager.InventorySlot> InventorySlotsFlat; //flat, not [,]
        public int InventoryRows;
        public int InventoryCols;
        public float DayManagerTimeElapsed;
        public int DayManagerDayCount;
        public bool HasCompletedFirstDay;
        public bool IsStartOfNewDay;
        //public float DayManagerUITransitionProgress;
        //public Vector3 PlayerFacingDirection;
        public int SympathyPoints;
        public List<QuestManager.QuestData> QuestDataList = new List<QuestManager.QuestData>();
        public int currencyAmount;
        public List<string> OwnedVanityItemIDs = new List<string>();
        public string EquippedVanityItemID;
        public float SoundVolume;
        public float MusicVolume;
        public bool HasQuestNotificationActive;
        public List<string> CollectedPickupIDs = new List<string>();
    }
    public SaveData GetSaveData()
    {
        SaveData saveData = new SaveData();
        FlattenInventorySlots(saveData);
        saveData.DayManagerTimeElapsed = DayManagerTimeElapsed;
        saveData.DayManagerDayCount = DayManagerDayCount;
        saveData.HasCompletedFirstDay = HasCompletedFirstDay;
        saveData.IsStartOfNewDay = IsStartOfNewDay;
        //saveData.DayManagerUITransitionProgress = 0; //loading data should always be at the start of the day
        //saveData.PlayerFacingDirection = PlayerFacingDirection;
        saveData.SympathyPoints = SympathyPoints;
        saveData.QuestDataList = QuestDataList;
        saveData.currencyAmount = currencyAmount;
        saveData.OwnedVanityItemIDs = OwnedVanityItemIDs;
        saveData.EquippedVanityItemID = EquippedVanityItemID;
        saveData.SoundVolume = SoundVolume;
        saveData.MusicVolume = MusicVolume;
        saveData.HasQuestNotificationActive = HasQuestNotificationActive;
        saveData.CollectedPickupIDs = CollectedPickupIDs;
        return saveData;
    }
    private void SetSaveData(SaveData data)
    {
        UnflattenInventorySlots(data);
        DayManagerTimeElapsed = data.DayManagerTimeElapsed;
        DayManagerDayCount = data.DayManagerDayCount;
        HasCompletedFirstDay = data.HasCompletedFirstDay;
        IsStartOfNewDay = data.IsStartOfNewDay;
        //DayManagerUITransitionProgress = data.DayManagerUITransitionProgress;
        //PlayerFacingDirection = data.PlayerFacingDirection;
        SympathyPoints = data.SympathyPoints;
        QuestDataList = data.QuestDataList;
        currencyAmount = data.currencyAmount;
        OwnedVanityItemIDs = data.OwnedVanityItemIDs;
        EquippedVanityItemID = data.EquippedVanityItemID;
        SoundVolume = data.SoundVolume;
        MusicVolume = data.MusicVolume;
        HasQuestNotificationActive = data.HasQuestNotificationActive;
        CollectedPickupIDs = data.CollectedPickupIDs;
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
        DayManagerDayCount = 0;
        HasCompletedFirstDay = false;
        IsStartOfNewDay = true;
        //DayManagerUITransitionProgress = 0;
        //PlayerFacingDirection = Vector3.zero;
        SympathyPoints = 0;
        QuestDataList.Clear();
        currencyAmount = 0;
        HasLoadedRunData = false;
        OwnedVanityItemIDs = new List<string>();
        EquippedVanityItemID = null;
        SoundVolume = 1f;
        MusicVolume = 1f;
        HasQuestNotificationActive = false;
        CollectedPickupIDs = new List<string>();
    }


    private void FlattenInventorySlots(SaveData data)
    {
        if (InventorySlots != null)
        {
            data.InventoryRows = InventorySlots.GetLength(0);
            data.InventoryCols = InventorySlots.GetLength(1);
            data.InventorySlotsFlat = new List<InventoryManager.InventorySlot>();
            for (int row = 0; row < data.InventoryRows; row++)
            {
                for (int col = 0; col < data.InventoryCols; col++)
                {
                    data.InventorySlotsFlat.Add(InventorySlots[row, col]);
                }
            }
        }
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