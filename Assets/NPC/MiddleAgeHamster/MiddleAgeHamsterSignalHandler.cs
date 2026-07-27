using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAgeHamsterSignalHandler : DialogueSignalHandler
{
    private MiddleAgeHamster hamsterNPC;
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {

        DialogueSignal signal = e as DialogueSignal;
        if (signal.signal == "middleAgeHamster_positive")
        {
            hamsterNPC = GetComponent<MiddleAgeHamster>();
            hamsterNPC.positiveDialogue = true;

        }
        else if (signal.signal == "middleAgeHamster_negative")
        {
            hamsterNPC = GetComponent<MiddleAgeHamster>();
            hamsterNPC.positiveDialogue = false;
        }
    }
}
