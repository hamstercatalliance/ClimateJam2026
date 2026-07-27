using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayEarlyHandler : DialogueSignalHandler
{
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        //DialogueRenderer.Instance.dialogueSignal += (sender, e) => {
            Debug.Log("DialogueRenderer: Received dialogue signal: " + e.ToString());
            if (e is DialogueSignal signal && signal.signal == "EndDayEarly")
            {
                DialogueBox.dialogueActive = false;
                DayManager.Instance.EndDay();
            }
            //};
    }
}
