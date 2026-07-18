using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayEarlyManager : MonoBehaviour
{
    private bool isSubscribed = false;
    // Start is called before the first frame update
    private void HandleDialogueSignal(System.Object sender, EventArgs e)
    {
        //DialogueRenderer.Instance.dialogueSignal += (sender, e) => {
            Debug.Log("DialogueRenderer: Received dialogue signal: " + e.ToString());
            if (e is DialogueSignal signal && signal.signal == "EndDayEarly")
            {
                DayManager.Instance.EndDay();
            }
        //};
    }
    private void OnEnable()
    {
        StartCoroutine(SubscribeToDialogue());
    }

    private IEnumerator SubscribeToDialogue() { 
        while (DialogueRenderer.Instance == null)
        {
            yield return null; // Wait for the next frame
        }
        Debug.Log("EndDayEarlyManager: Subscribing to dialogueSignal");
        DialogueRenderer.Instance.dialogueSignal += HandleDialogueSignal;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks and ghost crashes
        if (DialogueRenderer.Instance != null)
        {
            DialogueRenderer.Instance.dialogueSignal -= HandleDialogueSignal;
        }
    }

    // Update is called once per frame
}
