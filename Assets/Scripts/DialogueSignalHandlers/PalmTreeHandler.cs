using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmTreeHandler : DialogueSignalHandler
{
    [SerializeField] GameItem coconut;
    [SerializeField] Transform coconutSpawn;
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;
        if (dialogueSignal.signal == "shakeTree_confirm")
        {
            Instantiate(coconut, coconutSpawn.position, Quaternion.identity);
        }
    }
}
