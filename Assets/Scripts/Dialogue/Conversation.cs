using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ConversationPath {
    [JsonProperty("name")]
    public string name { get; set; }

    [JsonProperty("dialogue")]
    public List<DialogueBox> dialogue;
}

public class ConversationObject 
{
    [JsonProperty("startingPath")]
    public string startingPath { get; set; }
    [JsonProperty("data")]
    public List<ConversationPath> data = new List<ConversationPath>();
}



public class Conversation : MonoBehaviour
{
    // Start is called before the first frame update
    public List<DialogueBox> dialogue;
    private DialogueRenderer dialogueRenderer;
    public string dialogueName;

    public static Conversation Instance;

    private void Awake()
    {
        
        dialogueRenderer = FindFirstObjectByType<DialogueRenderer>();
        if (dialogueRenderer == null)
        {
            //Debug.LogError("Conversation: DialogueRenderer not found in the scene.");
        }
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void Execute(string dialogueName) {
        // Set to cannot move
        this.dialogueName = dialogueName;
        ChooseOption(null);
        DialogueBox.dialogueActive = true;
        StartCoroutine(DialogueLoop());
        IEnumerator DialogueLoop() {
            for (int i = 0; i < dialogue.Count; i++)
            {
                // Wait for dialogue to be inactive
                StartCoroutine(dialogueRenderer.Render(dialogue[i],this));
                yield return StartCoroutine(WaitForButtonPress(dialogue[i]));
                yield return StartCoroutine(Wait((dialogue[i].wait??0.0f)+0.1f));
            }
            ResetConverstation();
            DialogueBox.dialogueActive = false;
        }
        IEnumerator Wait(float delay) {
            yield return new WaitForSecondsRealtime(delay);
        }
        IEnumerator WaitForButtonPress(DialogueBox box) {
            while (box.active) {
                yield return null;
            }
        }
        // set to can move
    }

    public void CutOffDialogue()
    {
        StartCoroutine(dialogueRenderer.endDialogue(false));
    }

    public void AddDialogue(List<DialogueBox> dialogue) {
        if (dialogue.Count > 0)
        {
            dialogue[dialogue.Count - 1].lastBox = false;
        }
        this.dialogue.AddRange(dialogue);
        dialogue[dialogue.Count - 1].lastBox = true;
    }

    public void ChooseOption(string pathName=null) {
        // Load the next dialogue path from the JSON file
        TextAsset jsonAsset = Resources.Load<TextAsset>("DialogueFiles/" + dialogueName);

        if (jsonAsset != null)
        {
            string json = jsonAsset.text;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            ConversationObject conversationObject = JsonConvert.DeserializeObject<ConversationObject>(json, settings);
            // Find the starting path
            ConversationPath startingPath = conversationObject.data.Find(path => path.name == (pathName??conversationObject.startingPath));
            if (startingPath != null)
            {
                if (dialogue != null)
                {
                    AddDialogue(startingPath.dialogue);
                }
                else
                {
                    dialogue = startingPath.dialogue;
                }
                //StartDialogue();
            }
            else
            {
                //Debug.LogError($"Starting path '{conversationObject.startingPath}' not found in the dialogue file.");
            }
        }
        else
        {
            // Debug.LogError($"Dialogue file '{filePath}' not found.");
        }
    }

    public void ResetConverstation() {
        dialogue.Clear();
    }
}
