using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox {

    // Start is called before the first frame update

    private string dialogue;
    private string characterID;

    public static bool dialogueActive { get; set; } = false;
    public bool active { get; set; }

    public bool lastBox { get; set; }
    public float wait { get; private set; }

    public string? button1 { get; set; }
    public string? button2 { get; set; }

    public bool? button1press { get; set; } = null;


    public DialogueBox(string characterID, string dialogue, string? button1 = null, string? button2 = null, float? wait = null) { 
        this.dialogue = dialogue;
        this.characterID = characterID;
        this.active = true;
        this.wait = wait ?? 0.0f;
        this.button1 = button1;
        this.button2 = button2;
        lastBox = true;
    }

    public void setInactive()
    {
        this.active = false;
    }
     
    public string getContent() {
        return dialogue;
    }

    public string getCharacterID() { 
        return characterID;
    }
}
