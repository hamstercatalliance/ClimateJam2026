//csharp Assets\NPC\ChildCat\ChildCat.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCat : NonPlayableCharacter, IHasPersistentData
{
    public bool questCompleted;
    public bool questStarted;
    public bool hasInteracted;
    public bool positiveInteraction;
    private int dayInteracted;

    [SerializeField] private CoconutWaterQuest coconutWaterQuest;
    [SerializeField] private ColaQuest colaQuest;

    protected override void Init()
    {
        LoadGameData();
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    private void OnSceneTransitionHandler(object sender, EventArgs e)
    {
        WriteToGameData();
    }


    public bool DataSuccessfullyWritten { get; private set; }

    protected override void OnTalk()
    {
        // First-time interaction: record and return
        if (!hasInteracted)
        {
            hasInteracted = true;
            dayInteracted = DayManager.Instance.dayCount;
            conversation.Execute("ChildCat/Convo1");
            return;
        }

        if (!questStarted)
        {
            bool isLaterDay = DayManager.Instance.dayCount > dayInteracted;
            if (isLaterDay)
            {
                ExecuteDialogue("QuestStart");
            }
            else
            {
                ExecuteDialogue("1");
            }
            return;
        }

        // If quest started, handle completed vs in-progress responses
        if (questCompleted)
        {
            ExecuteDialogue("QuestPost");
        }
        else
        {
            GiveInitialQuestResponse();
        }
    }

    private void ExecuteDialogue(string name)
    {
        string qual = positiveInteraction ? "Pos" : "Neg";
        conversation.Execute(string.Format("ChildCat/Convo{0}{1}", name, qual));
    }

    public void setInteraction (bool interaction)
    {
        hasInteracted = true;
        positiveInteraction = interaction;
    }


    void GiveInitialQuestResponse()
    {
        Debug.Log("Coconute water quest initiated: " + coconutWaterQuest.isInitiated);
        if (coconutWaterQuest.isInitiated && coconutWaterQuest.CheckCocounutWater())
        {
            questCompleted = true;
            ExecuteDialogue("QuestEnd");
        }
        else if (colaQuest.isInitiated && colaQuest.CheckColas())
        {
            questCompleted = true;
            ExecuteDialogue("QuestEnd");
        } else
        {
            ExecuteDialogue("Quest");
        }
    }

    public void WriteToGameData()
    {
        GameData.Instance.ChildCatQuestCompleted = questCompleted;
        GameData.Instance.ChildCatQuestStarted = questStarted;
        GameData.Instance.ChildCatHasInteracted = hasInteracted;
        GameData.Instance.ChildCatPositiveInteraction = positiveInteraction;
        GameData.Instance.ChildCatDayInteracted = dayInteracted;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        questCompleted = GameData.Instance.ChildCatQuestCompleted;
        questStarted = GameData.Instance.ChildCatQuestStarted;
        hasInteracted = GameData.Instance.ChildCatHasInteracted;
        positiveInteraction = GameData.Instance.ChildCatPositiveInteraction;
        dayInteracted = GameData.Instance.ChildCatDayInteracted;
    }
}