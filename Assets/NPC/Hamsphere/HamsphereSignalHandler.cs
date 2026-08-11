using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HamsphereSignalHandler : DialogueSignalHandler
{
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
       //DialogueSignal dialogueSignal = e as DialogueSignal;
       // Conversation conversation = GetComponent<Hamsphere>().conversation;
       // if (dialogueSignal.signal == "hamsphere_1_2_proceed")
       // {
       //     conversation.Execute("Hamsphere/Convo1_2");
       // }
    }
}
