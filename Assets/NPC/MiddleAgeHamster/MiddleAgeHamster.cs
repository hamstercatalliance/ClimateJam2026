using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAgeHamster : NonPlayableCharacter
{
    public bool? positiveDialogue;
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
}
