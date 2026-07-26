using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHamsterSignalHandler : DialogueSignalHandler
{
    OneInteractNPC hamsterNPC;
    protected override void HandleDialogueSignal(object Object, System.EventArgs e)
    {

        DialogueSignal signal = e as DialogueSignal;
        if (signal.signal == "childHamster_positive")
        {
            hamsterNPC = GetComponent<OneInteractNPC>();
            hamsterNPC.conversation2 = "ChildHamster/ConvoNeg";

        }
        else if (signal.signal == "childHamster_negative") {
            hamsterNPC = GetComponent<OneInteractNPC>();
            hamsterNPC.conversation2 = "ChildHamster/ConvoPos";
        }
    }
}
