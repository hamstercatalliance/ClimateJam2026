using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class TestNPC1Script : NonPlayableCharacter
{

    // Start is called before the first frame update


    // Update is called once per frame
    public override void Init() {
        characterID = scriptableNPC.CharacterID;
        characterName = scriptableNPC.CharacterName;
    }

    public override void OnTalk()
    {
        Conversation conversation = gameObject.AddComponent<Conversation>();

        conversation.addDialogue(new DialogueBox(scriptableNPC.name, "I like cat"));
        conversation.Execute();
    }
}
