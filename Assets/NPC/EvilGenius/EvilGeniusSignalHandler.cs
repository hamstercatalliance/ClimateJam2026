using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGeniusSignalHandler : DialogueSignalHandler
{
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;

        if (dialogueSignal.signal == "evilGenius_StartJobBoardQuest")
        {
            OpenJobBoard.Instance.InitiateQuest();
        }
    }
}
