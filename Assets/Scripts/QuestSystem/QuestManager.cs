using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour, IHasPersistentData
{
    public static QuestManager Instance { get; private set; }
    [Serializable]
    public struct QuestData
    {
        public QuestSO questSO;
        public bool isCompleted;
        public bool isInitiated;
    }
    public List<QuestData> questDataList;//quest data for saving and loading, also contains all quests but excluding the instance
    
    public Quest[] quests;//for writing to the classes, all possible quests in the game should be added to this array in the inspector
    public bool DataSuccessfullyWritten { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        LoadGameData();
        Quest.OnQuestInitiated += Quest_OnQuestInitiated;
        Quest.OnQuestCompleted += Quest_OnQuestCompleted;
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
    }
    private void OnSceneTransitionHandler(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    private void Quest_OnQuestInitiated(object sender, EventArgs e)
    {
        Quest quest = sender as Quest;
        for (int i = 0; i < questDataList.Count; i++)
        {
            if (questDataList[i].questSO == quest.questSO)
            {
                QuestData questData = questDataList[i];
                questData.isInitiated = true;
                questDataList[i] = questData;
                QuestsUIManager.Instance.CreateQuestSlot(questData);
                break;
            }
        }
        //send data to quest UI to create a new quest slot for the initiated quest
    }
    private void Quest_OnQuestCompleted(object sender, EventArgs e)
    {
        Debug.Log("Quest completed event received in QuestManager");
        Quest quest = sender as Quest;
        //send data to quest UI to update the quest slot for the completed quest
        for (int i = 0; i < questDataList.Count; i++)
        {
            if (questDataList[i].questSO == quest.questSO)
            {
                QuestData questData = questDataList[i];
                questData.isCompleted = true;
                questDataList[i] = questData;
                QuestsUIManager.Instance.UpdateQuestUI(questData);
                break;
            }
        }
        
    }
    private void OnDestroy()
    {
        Quest.OnQuestInitiated -= Quest_OnQuestInitiated;
        Quest.OnQuestCompleted -= Quest_OnQuestCompleted;
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    public void LoadGameData()
    {
        Debug.Log("Loading quest data");
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            Debug.Log("Loading quest data from GameData");
            questDataList = GameData.Instance.QuestDataList;
            foreach (QuestData questData in questDataList)
            {
                foreach (Quest quest in quests)
                {
                    if (quest.questSO == questData.questSO)
                    {
                        if (questData.isInitiated)
                        {
                            quest.isInitiated = true;
                            if (questData.isCompleted)
                            {
                                quest.isCompleted = true;
                            }
                            Debug.Log("Creating quest slot for quest: " + questData.questSO.questName);
                            QuestsUIManager.Instance.CreateQuestSlot(questData);
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("No saved quest data found, initializing new quest data");
            questDataList = new List<QuestData>();
            for (int i = 0; i < quests.Length; i++)
            {
                QuestData questData = new QuestData
                {
                    questSO = quests[i].questSO,
                    isCompleted = quests[i].isCompleted,
                    isInitiated = quests[i].isInitiated
                };
                questDataList.Add(questData);
            }
        }
    }
    public void WriteToGameData()
    {
        GameData.Instance.QuestDataList = questDataList;
        DataSuccessfullyWritten = true;
    }
}
