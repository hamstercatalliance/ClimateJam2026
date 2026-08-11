using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GrocerSignalHandler : DialogueSignalHandler
{
    [SerializeField] private MerchantStore store;
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;
        if (dialogueSignal.signal == "grocer_openStore")
        {
            store.EnterStore();
        }
    }
    
}
