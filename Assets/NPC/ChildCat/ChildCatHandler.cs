using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCatHandler : DialogueSignalHandler
{

    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        ChildCat childCat = GetComponent<ChildCat>();
        DialogueSignal dialogueSignal = e as DialogueSignal;
        string signalString = dialogueSignal.signal;
        CoconutWaterQuest coconutWaterQuest = FindFirstObjectByType<CoconutWaterQuest>();
        ColaQuest colaQuest = FindFirstObjectByType<ColaQuest>();

        switch (signalString)
        {
            case "childCat_positive":
                childCat.setInteraction(true);
                break;
            case "childCat_negative":
                childCat.setInteraction(false);
                break;
            case "colaQuest_start":
                childCat.questStarted = true;
                coconutWaterQuest.InitiateQuest();
                break;
            case "colaQuest_end":
                childCat.questCompleted = true;
                coconutWaterQuest.CompleteQuest();
                break;
            case "coconutWaterQuest_start":
                childCat.questStarted = true;
                colaQuest.InitiateQuest();
                break;
            case "coconutWaterQuest_end":
                childCat.questCompleted = true;
                colaQuest.CompleteQuest();
                break;
            default:
                break;
            }
        }
}
