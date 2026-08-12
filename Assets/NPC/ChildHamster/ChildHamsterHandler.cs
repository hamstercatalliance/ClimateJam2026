using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHamsterHandler : DialogueSignalHandler
{
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;
        if (dialogueSignal.signal == "childHamster_negative")
        {
            GetComponent<ChildHamster>().SetInteraction(false);
        }
        else if (dialogueSignal.signal == "childHamster_positive")
        {
            GetComponent<ChildHamster>().SetInteraction(true);
        }
    }
}
