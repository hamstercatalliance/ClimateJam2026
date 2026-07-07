using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox {

    // Start is called before the first frame update

    private string dialogue;
    private string characterID;


    public bool active { get; set; }
    public float wait { get; private set; }


    public DialogueBox(string dialogue, string characterID,  float wait) { 
        this.dialogue = dialogue;
        this.characterID = characterID;
        this.active = true;
        this.wait = wait;
    }

    public void setInactive()
    {
        this.active = false;
    }
}
