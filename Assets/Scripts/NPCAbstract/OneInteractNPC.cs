using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneInteractNPC : NonPlayableCharacter
{
    // Start is called before the first frame update
    [SerializeField] public string conversation1;
    [SerializeField] public string conversation2;
    bool hasInteracted = false;
    protected override void OnTalk()
    {
        if (!hasInteracted)
        {
            conversation.Execute(conversation1);
            hasInteracted = true;
        }
        else
        {
            conversation.Execute(conversation2);
        } // TODO: Add to save data
    }
}
