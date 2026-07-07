using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox {
{
    // Start is called before the first frame update

    [SerializeField] string dialogue;
    [SerializeField] string characterID;

    public bool active { get; set; }
    public float wait { get; private set; }
    public DialogueBox() { 
    
    }
    public void Execute() { 
    
    }
}
