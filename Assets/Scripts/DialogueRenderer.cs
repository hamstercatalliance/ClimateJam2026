using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System;

public class DialogueSignal : EventArgs
{
    public string signal;
    public DialogueSignal(string signal)
    {
        this.signal = signal;
    }
}

public class AddPointsDialogueSignal : DialogueSignal 
{
    public int points;
    public AddPointsDialogueSignal (string signal, int points) : base(signal)
    {
        this.points = points;
    }
}



public class DialogueRenderer : MonoBehaviour
{
    //GameObject box;
    GameInput gameInput;
    //public EventHandler buttonPress { get; private set; }
    public EventHandler dialogueSignal { get; set; }
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject box;

    public static DialogueRenderer Instance { get; private set; } 
    private void Start()
    {
        gameInput = FindFirstObjectByType<GameInput>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public IEnumerator Render(DialogueBox dialogueObject, Conversation conversation) {
        //Instantiate(box);
        Debug.Log("DialogueRenderer: Rendering dialogue box for character " + dialogueObject.getCharacterID() + " with content: " + dialogueObject.getContent());
        //box = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Textbox.prefab");
        GameObject dialoguePanel = Instantiate(box, GameObject.Find("Canvas").transform);


        Transform characterText = dialoguePanel.transform.Find("CharacterText");
        Transform contentText = dialoguePanel.transform.Find("ContentText");

        Transform optionsContainer = dialoguePanel.transform.Find("Container");


        contentText.GetComponent<TMP_Text>().text = dialogueObject.getContent();
        characterText.GetComponent<TMP_Text>().text = dialogueObject.getCharacterID();
        bool hasOptions = dialogueObject.buttons.Count > 0;
        
        dialogueObject.active = true;
        
        //GameObject buttonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ButtonTemplate.prefab");

        if (hasOptions) 
        {
            optionsContainer.gameObject.SetActive(true);
            foreach (DialogueButton button in dialogueObject.buttons)
            {
                GameObject optionButton = Instantiate(buttonPrefab, optionsContainer);
                optionButton.GetComponentInChildren<TMP_Text>().text = button.text;
                optionButton.GetComponent<Button>().onClick.AddListener(() => {
                    StartCoroutine(endDialogue(dialogueObject, dialoguePanel));
                    Debug.Log("DialogueRenderer: Button clicked with text: " + button.text + " and path: " + button.path);
                    SendDialogueSignal(button.id, dialogueObject.sympathyPointsChange);
                    if (button.path != null && button.path != "")
                    {
                        conversation.ChooseOption(button.path);
                    }
                });
            }
        }
        else { 
            optionsContainer.gameObject.SetActive(false);
        }

            yield return new WaitForSecondsRealtime(0.1f); // Wait for 0.1 seconds to ensure the dialogue box is rendered before proceeding
        gameInput.OnInteractAction += (sender, e) => {
            if (dialogueObject.active )
            {
                if (dialogueObject.signal != null && dialogueObject.signal != "")
                {
                    Debug.Log("DialogueRenderer: Interact key pressed, sending signal for character " + dialogueObject.getCharacterID() + " with content: " + dialogueObject.getContent() + " and signal: " + dialogueObject.signal);
                    SendDialogueSignal(dialogueObject.signal, dialogueObject.sympathyPointsChange);
                }
                if (!hasOptions)
                {
                    //Debug.Log("DialogueRenderer: Interact key pressed, ending dialogue box for character " + dialogueObject.getCharacterID() + " with content: " + dialogueObject.getContent());

                    StartCoroutine(endDialogue(dialogueObject, dialoguePanel));
                }
            }
        };

    }

    private void SendDialogueSignal(string signal, int? points = null) {
        Debug.Log("Sending dialogue signal: " + signal + " with points: " + (points.HasValue ? points.Value.ToString() : "null"));
        if (points == null || points == 0)
        {
            dialogueSignal?.Invoke(this, new DialogueSignal(signal));
        }
        else if (signal == "addPoints") 
        { 
            dialogueSignal?.Invoke(this, new AddPointsDialogueSignal(signal, points.Value));
        }
    }

    private IEnumerator endDialogue(DialogueBox dialogue, GameObject dialoguePanel) {
        yield return new WaitForSecondsRealtime(0.1f); // Wait for 0.1 seconds to ensure the dialogue box is rendered before proceeding
        dialogue.setInactive();
        Debug.Log("DialogueRenderer: Ending dialogue box for character " + dialogue.getCharacterID() + " with content: " + dialogue.getContent());
        //if (dialogue.lastBox)
        //{
        //    DialogueBox.dialogueActive = false;
        //}
        Destroy(dialoguePanel);
    }
}  