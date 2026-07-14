using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestNPC1Script : NonPlayableCharacter
{

    // Start is called before the first frame update


    // Update is called once per frame
    public override void Init() {

    }

    public override void OnTalk()
    {
        Conversation conversation = gameObject.AddComponent<Conversation>();
        conversation.Execute("TestConversation1");

    }
}
