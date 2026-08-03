using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenJobBoard : Quest
{
    public bool hasOpenedJobBoard { get; private set; }
    public static OpenJobBoard Instance;

    protected override void Start()
    {
        base.Start();
        InitiateQuest();
        Debug.Log(isInitiated.ToString());
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public override void WriteToGameData()
    {
        GameData.Instance.OpenedQuestBoard = hasOpenedJobBoard;
        base.WriteToGameData();
    }

    public override void LoadGameData()
    {
        if (GameData.Instance != null)
        {
            setCondition(GameData.Instance.OpenedQuestBoard);
        }
        base.LoadGameData();
    }

    public void setCondition(bool condition=true) {
        hasOpenedJobBoard = condition;
        isCompleted = condition;
        WriteToGameData();
    }
}   
