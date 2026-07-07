using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogueRenderer : MonoBehaviour
{
    static GameObject box;

    public static void Render(DialogueBox dialogueObject) {
        //Instantiate(box);
        box = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Textbox.prefab");
        GameObject dialoguePanel = Instantiate(box, GameObject.Find("Canvas").transform);

        Transform characterText = dialoguePanel.transform.Find("CharacterText");
        Transform contentText = dialoguePanel.transform.Find("ContentText");
        Transform dialogueButton1 = dialoguePanel.transform.Find("DialogueButton1");
        Transform dialogueButton2 = dialoguePanel.transform.Find("DialogueButton2");

        

        // Basic dialogue listener
        dialogueButton1.GetComponent<Button>().onClick.AddListener(() => { 
            Destroy(dialoguePanel);
            dialogueObject.setInactive();
        });
        // End basic dialogue listener
    }
}
