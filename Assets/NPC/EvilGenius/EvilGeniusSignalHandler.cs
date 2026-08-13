using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGeniusSignalHandler : DialogueSignalHandler
{
    private EvilGenius evilGenius;
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;

        if (dialogueSignal.signal == "evilGenius_StartJobBoardQuest")
        {
            OpenJobBoard.Instance.InitiateQuest();
        }
        else if (dialogueSignal.signal == "evilGenius_positive")
        {
            evilGenius = GetComponent<EvilGenius>();
            evilGenius.positiveDialogue = true;

        }
        else if (dialogueSignal.signal == "evilGenius_negative")
        {
            evilGenius = GetComponent<EvilGenius>();
            evilGenius.positiveDialogue = false;
        }
    }
}
