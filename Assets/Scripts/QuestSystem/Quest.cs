using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Quest : MonoBehaviour, IHasPersistentData
{
    public QuestSO questSO;
    public bool isInitiated = false;
    public bool isCompleted;
    public static event EventHandler OnQuestInitiated;
    public static event EventHandler OnQuestCompleted;
    public bool DataSuccessfullyWritten { get; private set; }
    protected virtual void Start()
    {
        LoadGameData();
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
    }
    protected virtual void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    protected virtual void OnSceneTransitionHandler(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    public virtual void InitiateQuest()
    {
        if (!isInitiated)
        {
            isInitiated = true;
            isCompleted = false;
            OnQuestInitiated?.Invoke(this, EventArgs.Empty);
        }
    }
    public virtual void CompleteQuest()
    {
        if (isCompleted)
        {
            return;
        }
        isCompleted = true;
        OnQuestCompleted?.Invoke(this, EventArgs.Empty);
        
        CurrencyManager.Instance.AddCurrency(questSO.currencyAward);
        if (questSO.currencyAward > 0)
        {
            SoundManager.Instance.PlayCoinsSound();
        }
        SympathyPointsManager.Instance.AddSympathyPoints(questSO.sympathyAward);
    }
    public virtual void WriteToGameData()
    {
        DataSuccessfullyWritten = true;
    }
    public virtual void LoadGameData()
    {
        //QUESTS DO NOT NEED TO WORRY ABOUT LOADING IS INITIATED OR IS COMPLETED
        //IS INITIATED AND IS COMPLETED WILL BE SET IN QUESTMANAGER WHEN LOADING
        //THIS IS FOR QUEST SPECIFIC PROGRESS DATA THAT NEEDS TO BE SAVED AND LOADED
    }
}
