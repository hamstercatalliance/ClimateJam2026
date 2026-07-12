using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Conversation : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueBox[] dialogue = new DialogueBox[0];
    private DialogueRenderer dialogueRenderer;

    private void Awake()
    {
        
        dialogueRenderer = FindFirstObjectByType<DialogueRenderer>();
        if (dialogueRenderer == null)
        {
            Debug.LogError("Conversation: DialogueRenderer not found in the scene.");
        }
    }

    public void Execute() {
        // Set to cannot move 
        DialogueBox.dialogueActive = true;
        StartCoroutine(DialogueLoop());
        IEnumerator DialogueLoop () {
            for (int i = 0; i < dialogue.Length; i++)
            {
                // Wait for dialogue to be inactive
                StartCoroutine(dialogueRenderer.Render(dialogue[i]));
                //while (true) 
                //{
                //    if (!dialogue[i].active)
                //    {
                //        break;
                //    }
                //}
                yield return StartCoroutine(WaitForButtonPress(dialogue[i]));
                yield return StartCoroutine(Wait(dialogue[i].wait));
            }
        }
        IEnumerator Wait(float delay) { 
            yield return new WaitForSecondsRealtime(delay);
        }
        IEnumerator WaitForButtonPress (DialogueBox box) { 
            while (box.active) {
                yield return null;
            }
        }
        // set to can move
    }

    public void addDialogue(DialogueBox dialogueBox) {
        if (dialogue.Length > 0)
        {
            dialogue[dialogue.Length - 1].lastBox = false;
        }
        Array.Resize(ref dialogue, dialogue.Length+1);
        dialogue[dialogue.Length-1] = dialogueBox;
    }

    public void ResetConverstation() {
        Array.Resize(ref dialogue, 0);   
    }
}
