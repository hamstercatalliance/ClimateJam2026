using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGenius : NonPlayableCharacter
{
    protected override void OnTalk() {
        conversation.Execute("EvilGenius/Convo1");
    }
}
