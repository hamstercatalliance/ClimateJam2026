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

    //private void Start()
    //{
    //    dialogue = new DialogueBox[0];
    //}

    public void Execute() {
        // Set to cannot move 
        StartCoroutine(DialogueLoop());
        IEnumerator DialogueLoop () {
            for (int i = 0; i < dialogue.Length; i++)
            {
                // Wait for dialogue to be inactive
                DialogueRenderer.Render(dialogue[i]);
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
        Array.Resize(ref dialogue, dialogue.Length+1);
        dialogue[dialogue.Length-1] = dialogueBox;
    }
}
