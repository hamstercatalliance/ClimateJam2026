using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishercatHandler : DialogueSignalHandler
{
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;
        if (dialogueSignal != null && dialogueSignal.signal == "fisherCat_openStore")
        {
            Fishercat fishercat = GetComponent<Fishercat>();
            if (fishercat != null)
            {
                fishercat.fishingStore.EnterStore();
            }
        }
    }
}
