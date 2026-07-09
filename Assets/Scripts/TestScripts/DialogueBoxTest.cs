using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Conversation conversation = gameObject.AddComponent<Conversation>();
        conversation.addDialogue(new DialogueBox("Sam", "I sit"));
        conversation.addDialogue(new DialogueBox("Anderdingu", "bleh", 2.0f));
        conversation.addDialogue(new DialogueBox("Sam", "YIPEEEEEEEE"));

        conversation.Execute();
    }
}
