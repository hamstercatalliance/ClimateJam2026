using System;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

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
    public float EndOfDayCountdownTimeElapsed;
    public bool IsCountingDown = false;
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
    public bool FlowerSeedsPickedUp;
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
    #region NPC Data
    #region Child Cat
    public bool ChildCatQuestCompleted;
    public bool ChildCatQuestStarted;
    public bool ChildCatHasInteracted;
    public bool ChildCatPositiveInteraction;
    public int ChildCatDayInteracted;
    #endregion
    #region MiddleAgeHamster
    public bool MiddleAgeHamsterPositiveInteraction;
    public bool MiddleAgeHamsterHasInteracted;
    #endregion
    #region Child Hamster
    public bool ChildHamsterPositiveConvo;
    public bool ChildHamsterHasInteracted;
    public int ChildHamsterDayInteracted;
    #endregion
    #region Evil Genius
    public bool EvilGeniusPositiveInteraction;
    public bool EvilGeniusHasInteracted;
    #endregion
    #region Groster
    public bool GrosterHasInteracted;
    #endregion
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
        public bool ChildCatQuestCompleted;
        public bool ChildCatQuestStarted;
        public bool ChildCatHasInteracted;
        public bool ChildCatPositiveInteraction;
        public bool FlowerSeedsPickedUp;
        public int ChildCatDayInteracted;
        public bool MiddleAgeHamsterPositiveInteraction;
        public bool MiddleAgeHamsterHasInteracted;
        public bool EvilGeniusPositiveInteraction;
        public bool EvilGeniusHasInteracted;
        public bool ChildHamsterPositiveConvo;
        public bool ChildHamsterHasInteracted;
        public int ChildHamsterDayInteracted;
        public bool GrosterHasInteracted;
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
        saveData.ChildCatQuestCompleted = ChildCatQuestCompleted;
        saveData.ChildCatQuestStarted = ChildCatQuestStarted;
        saveData.ChildCatHasInteracted = ChildCatHasInteracted;
        saveData.ChildCatPositiveInteraction = ChildCatPositiveInteraction;
        saveData.ChildCatDayInteracted = ChildCatDayInteracted;
        saveData.MiddleAgeHamsterPositiveInteraction = MiddleAgeHamsterPositiveInteraction;
        saveData.MiddleAgeHamsterHasInteracted = MiddleAgeHamsterHasInteracted;
        saveData.EvilGeniusPositiveInteraction = EvilGeniusPositiveInteraction;
        saveData.EvilGeniusHasInteracted = EvilGeniusHasInteracted;
        saveData.FlowerSeedsPickedUp = FlowerSeedsPickedUp;
        saveData.ChildHamsterPositiveConvo = ChildHamsterPositiveConvo;
        saveData.ChildHamsterHasInteracted = ChildHamsterHasInteracted;
        saveData.ChildHamsterDayInteracted = ChildHamsterDayInteracted;
        saveData.GrosterHasInteracted = GrosterHasInteracted;
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
        ChildCatQuestCompleted = data.ChildCatQuestCompleted;
        ChildCatQuestStarted = data.ChildCatQuestStarted;
        ChildCatHasInteracted = data.ChildCatHasInteracted;
        ChildCatPositiveInteraction = data.ChildCatPositiveInteraction;
        ChildCatDayInteracted = data.ChildCatDayInteracted;
        MiddleAgeHamsterPositiveInteraction = data.MiddleAgeHamsterPositiveInteraction;
        MiddleAgeHamsterHasInteracted = data.MiddleAgeHamsterHasInteracted;
        EvilGeniusPositiveInteraction = data.EvilGeniusPositiveInteraction;
        EvilGeniusHasInteracted = data.EvilGeniusHasInteracted;
        FlowerSeedsPickedUp = data.FlowerSeedsPickedUp;
        ChildHamsterPositiveConvo = data.ChildHamsterPositiveConvo;
        ChildHamsterHasInteracted = data.ChildHamsterHasInteracted;
        ChildHamsterDayInteracted = data.ChildHamsterDayInteracted;
        GrosterHasInteracted = data.GrosterHasInteracted;
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
        ChildCatQuestCompleted = false;
        ChildCatQuestStarted = false;
        ChildCatHasInteracted = false;
        ChildCatPositiveInteraction = false;
        ChildCatDayInteracted = 0;
        MiddleAgeHamsterPositiveInteraction = false;
        MiddleAgeHamsterHasInteracted = false;
        EvilGeniusPositiveInteraction = false;
        EvilGeniusHasInteracted = false;
        FlowerSeedsPickedUp = false;
        ChildHamsterPositiveConvo = false;
        ChildHamsterHasInteracted = false;
        ChildHamsterDayInteracted = 0;
        GrosterHasInteracted = false;
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