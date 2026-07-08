using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Conversation : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueBox[] dialogue;



    public void Execute() {
        // Set to cannot move 
        for (int i = 0; i < dialogue.Length; i++)
        {
            // Wait for dialogue to be inactive
            while (true) 
            {
                if (!dialogue[i].active)
                {
                    break;
                }
            }
            StartCoroutine(Wait(dialogue[i].wait));
        }
        IEnumerator Wait (float delay) {
            yield return new WaitForSecondsRealtime(delay);
        }
        // set to can move
    }
}
