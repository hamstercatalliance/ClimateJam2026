using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
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

        dialogueButton2.gameObject.SetActive(!(dialogueObject.button2 == null));

        contentText.GetComponent<TMP_Text>().text = dialogueObject.getContent();
        characterText.GetComponent<TMP_Text>().text = dialogueObject.getCharacterID();

        dialogueButton1.GetChild(0).GetComponent<TMP_Text>().text = dialogueObject.button1;
        dialogueButton2.GetChild(0).GetComponent<TMP_Text>().text = dialogueObject.button2;

        // Basic dialogue listener
        dialogueButton1.GetComponent<Button>().onClick.AddListener(() => {
            endDialogue(dialogueObject, dialoguePanel);
            dialogueObject.button1press = true;
        });
        dialogueButton2.GetComponent<Button>().onClick.AddListener(() => {
            endDialogue(dialogueObject, dialoguePanel);
            dialogueObject.button1press = false;
        });
        // End basic dialogue listener
    }
    private static void endDialogue(DialogueBox dialogue, GameObject dialoguePanel) {
        dialogue.setInactive();
        if (dialogue.lastBox)
        {
            DialogueBox.dialogueActive = false;
        }
        Destroy(dialoguePanel);
    }
}  