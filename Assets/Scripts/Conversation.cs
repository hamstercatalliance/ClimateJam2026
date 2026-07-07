using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueBox[] dialogue;



    public void Execute() {
        // Set to cannot move 
        for (int i = 0; i < dialogue.Length; i++)
        {
            DialogueRenderer.Execute(dialogue[i]);
            // Wait for dialogue to be inactive
            bool continueDialogue = false;
            while (!continueDialogue)
            {
                if (!dialogue[i].active)
                {
                    continueDialogue = true;
                }
            }
        }
        // set to can move
    }
}
