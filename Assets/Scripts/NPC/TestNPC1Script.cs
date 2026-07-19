using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestNPC1Script : NonPlayableCharacter
{

    // Start is called before the first frame update


    // Update is called once per frame

    protected override void OnTalk()
    {

        // Conversation conversation = FindFirstObjectByType<Conversation>();
        conversation.Execute("TestConversation1");

    }
}
