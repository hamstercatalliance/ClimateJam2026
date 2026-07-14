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
        DialogueBox testOptions = new DialogueBox(scriptableNPC.name, "I like cat");
        testOptions.addButton("Option 1", () => { 
            Debug.Log("Option 1 selected"); 
        });
        //testOptions.addButton("Option 2", () =>
        //{
        //    Debug.Log("Option 2 selected");
        //});
        //testOptions.addButton("Option 3", () =>
        //{
        //    Debug.Log("Option 3 selected");
        //});
        //testOptions.addButton("Option 4", () =>
        //{
        //    Debug.Log("Option 4 selected");
        //});
        //testOptions.addButton("Option 5", () =>
        //{
        //    Debug.Log("Option 5 selected");
        //});
        //testOptions.addButton("Option 6", () =>
        //{
        //    Debug.Log("Option 6 selected");
        //});
        conversation.addDialogue(testOptions);
        conversation.Execute();
    }
}
