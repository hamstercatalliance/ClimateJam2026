using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneInteractSignalHandler : DialogueSignalHandler
{
    private OneInteractNPC npc;
    [SerializeField] private string posSignal;
    [SerializeField] private string negSignal;
    [SerializeField] private string posDialogue;
    [SerializeField] private string negDialogue;
    protected override void HandleDialogueSignal(object Object, System.EventArgs e)
    {

        DialogueSignal signal = e as DialogueSignal;
        if (signal.signal == posSignal)
        {
            npc = GetComponent<OneInteractNPC>();
            npc.conversation2 = posDialogue;

        }
        else if (signal.signal == negSignal) {
            npc = GetComponent<OneInteractNPC>();
            npc.conversation2 = negDialogue;
        }
    }
}
