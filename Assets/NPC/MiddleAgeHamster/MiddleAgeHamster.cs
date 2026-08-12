using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAgeHamster : NonPlayableCharacter, IHasPersistentData
{
    public bool? positiveDialogue;

    protected override void Init()
    {
        base.Init();
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
        string dialogueName;
        if (positiveDialogue == null)
        {
            switch (DayManager.Instance.dayCount)
            {
                case 0:
                    dialogueName = "MiddleAgeHamster/Convo1";
                    break;
                default:
                    dialogueName = "MiddleAgeHamster/Convo2";
                    break;
            }
        }
        else
        {
            dialogueName = positiveDialogue.Value ? "MiddleAgeHamster/Convo2pos" : "MiddleAgeHamster/Convo2neg";
        }
            conversation.Execute(dialogueName);
        
    }
    public void WriteToGameData()
    {
        GameData.Instance.MiddleAgeHamsterPositiveInteraction = positiveDialogue == null? false : positiveDialogue.Value;
        GameData.Instance.MiddleAgeHamsterHasInteracted = positiveDialogue != null;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        positiveDialogue = GameData.Instance.MiddleAgeHamsterHasInteracted ? (bool?)GameData.Instance.MiddleAgeHamsterPositiveInteraction : null;
    }
}
