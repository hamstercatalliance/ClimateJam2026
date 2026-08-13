using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grocer : NonPlayableCharacter, IHasPersistentData
{
    private bool hasInteracted = false;
    public bool DataSuccessfullyWritten { get; private set; }
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
    protected override void OnTalk()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            conversation.Execute("Grocer/Convo1");
        }
        else
        {
            conversation.Execute("Grocer/Convo1_2");
        }
    }
    public void WriteToGameData()
    {
        GameData.Instance.GrosterHasInteracted = hasInteracted;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        hasInteracted = GameData.Instance.GrosterHasInteracted;
    }
}
