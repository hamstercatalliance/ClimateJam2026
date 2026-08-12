using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHamster : NonPlayableCharacter, IHasPersistentData
{
    public bool positiveConvo;
    public bool hasInteracted;
    public int dayInteracted;

    public bool DataSuccessfullyWritten {  get; private set; }

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
    protected override void OnTalk() {
        if (dayInteracted >= DayManager.Instance.dayCount || hasInteracted == false) {
            firstDayInteraction();
        } else
        {
            laterInteraction();
        }
           
     
    }
    public void SetInteraction(bool qual)
    {
        positiveConvo = qual;
        hasInteracted = true;
        dayInteracted = DayManager.Instance.dayCount;
    }

    private void firstDayInteraction ()
    {
        if (!hasInteracted)
        {
            conversation.Execute("ChildHamster/Convo1");
        }
        else
        {
            if (positiveConvo)
            {
                conversation.Execute("ChildHamster/ConvoPos");
            }
            else
            {
                conversation.Execute("ChildHamster/ConvoNeg");
            }
        }
    }

    private void laterInteraction()
    {
        if (positiveConvo)
        {
            conversation.Execute("ChildHamster/Convo2Pos");
        }
        else
        {
            conversation.Execute("ChildHamster/Convo2Neg");
        }
    }

    public void WriteToGameData()
    {
        GameData.Instance.ChildHamsterPositiveConvo = positiveConvo;
        GameData.Instance.ChildHamsterHasInteracted = hasInteracted;
        GameData.Instance.ChildHamsterDayInteracted = dayInteracted;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        positiveConvo = GameData.Instance.ChildHamsterPositiveConvo;
        hasInteracted = GameData.Instance.ChildHamsterHasInteracted;
        dayInteracted = GameData.Instance.ChildHamsterDayInteracted;
    }
}
