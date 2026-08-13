using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGenius : NonPlayableCharacter
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
                    dialogueName = "EvilGenius/Convo1";
                    break;
                case 1:
                    dialogueName = "EvilGenius/Convo2";
                    break;
                default:
                    dialogueName = "EvilGenius/Convo3";
                    break;
            }
        }
        else
        {
            dialogueName = positiveDialogue.Value ? "EvilGenius/Convo3pos" : "EvilGenius/Convo3neg";
        }
        conversation.Execute(dialogueName);

    }
    public void WriteToGameData()
    {
        GameData.Instance.EvilGeniusPositiveInteraction = positiveDialogue == null ? false : positiveDialogue.Value;
        GameData.Instance.EvilGeniusHasInteracted = positiveDialogue != null;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        positiveDialogue = GameData.Instance.EvilGeniusHasInteracted ? (bool?)GameData.Instance.EvilGeniusPositiveInteraction : null;
    }
}
