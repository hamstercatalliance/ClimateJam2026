using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNotifyManager : MonoBehaviour, IHasPersistentData
{
    [SerializeField] private GameObject questNotification;
    private bool isNotificationActive = false;
    public bool DataSuccessfullyWritten { get; private set; }
    public static QuestNotifyManager Instance { get; private set; }
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
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        Quest.OnQuestInitiated += QuestManager_OnQuestInitiated;
    }
    private void QuestManager_OnQuestInitiated(object sender, System.EventArgs e)
    {
        ShowQuestNotification();
    }
    private void OnSceneTransitionHandler(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
        Quest.OnQuestInitiated -= QuestManager_OnQuestInitiated;
    }
    public void ShowQuestNotification()
    {
        questNotification.SetActive(true);
        isNotificationActive = true;
    }
    public void HideQuestNotification()
    {
        questNotification.SetActive(false);
        isNotificationActive = false;
    }
    public void WriteToGameData()
    {
        GameData.Instance.HasQuestNotificationActive = isNotificationActive;
        DataSuccessfullyWritten = true;
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            isNotificationActive = GameData.Instance.HasQuestNotificationActive;
            questNotification.SetActive(isNotificationActive);
        }
        else
        {
            isNotificationActive = false;
            questNotification.SetActive(false);
        }
    }
}
