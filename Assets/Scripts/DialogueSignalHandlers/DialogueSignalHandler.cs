using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueSignalHandler : MonoBehaviour
{
    // Start is called before the first frame update
    protected abstract void HandleDialogueSignal(System.Object sender, EventArgs e);
    protected void OnEnable()
    {
        StartCoroutine(SubscribeToDialogue());
    }

    protected IEnumerator SubscribeToDialogue() { 
        while (DialogueRenderer.Instance == null)
        {
            yield return null; // Wait for the next frame
        }
        //Debug.Log("EndDayEarlyManager: Subscribing to dialogueSignal");
        DialogueRenderer.Instance.dialogueSignal += HandleDialogueSignal;
    }

    protected void OnDisable()
    {
        if (DialogueRenderer.Instance != null)
        {
            DialogueRenderer.Instance.dialogueSignal -= HandleDialogueSignal;
        }
    }

    // Update is called once per frame
}
