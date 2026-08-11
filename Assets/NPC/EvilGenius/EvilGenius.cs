using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGenius : NonPlayableCharacter
{
    protected override void OnTalk() {
        switch (DayManager.Instance.dayCount) {
            case 0:
                conversation.Execute("EvilGenius/Convo1");
                break;
            case 1:
                conversation.Execute("EvilGenius/Convo2");
                break;
            case 2:
                conversation.Execute("EvilGenius/Convo3");
                break;
        }
    }
}
