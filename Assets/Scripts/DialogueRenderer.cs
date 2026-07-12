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
    GameObject box;
    GameInput gameInput;

    private void Start()
    {
        gameInput = FindFirstObjectByType<GameInput>();
    }

    public IEnumerator Render(DialogueBox dialogueObject) {
        //Instantiate(box);
        Debug.Log("DialogueRenderer: Rendering dialogue box for character " + dialogueObject.getCharacterID() + " with content: " + dialogueObject.getContent());
        box = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Textbox.prefab");
        GameObject dialoguePanel = Instantiate(box, GameObject.Find("Canvas").transform);


        Transform characterText = dialoguePanel.transform.Find("CharacterText");
        Transform contentText = dialoguePanel.transform.Find("ContentText");
        //Transform dialogueButton1 = dialoguePanel.transform.Find("DialogueButton1");
        //Transform dialogueButton2 = dialoguePanel.transform.Find("DialogueButton2");
        Transform optionsContainer = dialoguePanel.transform.Find("Container");

        //dialogueButton2.gameObject.SetActive(!(dialogueObject.button2 == null));

        contentText.GetComponent<TMP_Text>().text = dialogueObject.getContent();
        characterText.GetComponent<TMP_Text>().text = dialogueObject.getCharacterID();
        bool hasOptions = dialogueObject.buttons.Count > 0;

        GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ButtonTemplate.prefab");

        if (hasOptions) 
        {
            optionsContainer.gameObject.SetActive(true);
            foreach (KeyValuePair<string, System.Action> button in dialogueObject.buttons)
            {
                GameObject optionButton = Instantiate(buttonPrefab, optionsContainer);
                optionButton.GetComponentInChildren<TMP_Text>().text = button.Key;
                optionButton.GetComponent<Button>().onClick.AddListener(() => {
                    StartCoroutine(endDialogue(dialogueObject, dialoguePanel));
                    button.Value.Invoke();
                });
            }
        }
        else { 
            optionsContainer.gameObject.SetActive(false);
        }

            yield return new WaitForSecondsRealtime(0.1f); // Wait for 0.1 seconds to ensure the dialogue box is rendered before proceeding
        gameInput.OnInteractAction += (sender, e) => {
            if (dialogueObject.active && !hasOptions)
            {
                Debug.Log("DialogueRenderer: Interact key pressed, ending dialogue box for character " + dialogueObject.getCharacterID() + " with content: " + dialogueObject.getContent());
                StartCoroutine(endDialogue(dialogueObject, dialoguePanel));
                dialogueObject.button1press = true;
            }
        };
        // Basic dialogue listener
        //dialogueButton1.GetComponent<Button>().onClick.AddListener(() => {
        //    StartCoroutine(endDialogue(dialogueObject, dialoguePanel));
        //    dialogueObject.button1press = true;
        //});
        // Listen for "interact" key press and set dialogueObject.button1press to true

        // Options:
        // DialogueBox will have dictionary of strings and functions, and dialogueRenderer will create an array of buttons with the strings as text and the functions as onClick listeners. The functions will be called when the buttons are pressed, and the dialogue box will be closed.


        //dialogueButton2.GetComponent<Button>().onClick.AddListener(() => {
        //    StartCoroutine(endDialogue(dialogueObject, dialoguePanel));
        //    dialogueObject.button1press = false;
        //});
        // End basic dialogue listener

        //IEnumerator Wait(float delay) { 
        //    yield return new WaitForSecondsRealtime(dialogueObject.wait);
        //}

    }

    private IEnumerator endDialogue(DialogueBox dialogue, GameObject dialoguePanel) {
        yield return new WaitForSecondsRealtime(0.1f); // Wait for 0.1 seconds to ensure the dialogue box is rendered before proceeding
        dialogue.setInactive();
        Debug.Log("DialogueRenderer: Ending dialogue box for character " + dialogue.getCharacterID() + " with content: " + dialogue.getContent());
        if (dialogue.lastBox)
        {
            DialogueBox.dialogueActive = false;
        }
        Destroy(dialoguePanel);
    }
}  