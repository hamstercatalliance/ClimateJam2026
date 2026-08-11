using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grocer : NonPlayableCharacter
{
    private bool hasInteracted = false;
    protected override void OnTalk()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            conversation.Execute("Grocer/Convo1");
        }
        else
        {
            conversation.Execute("Grocer/Convo1_2");
        }
    }
}
