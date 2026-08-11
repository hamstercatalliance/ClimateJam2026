using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OldCat : NonPlayableCharacter
{
    protected override void OnTalk()
    {
        BeachCleanup beachCleanup = FindObjectOfType<BeachCleanup>();
        switch (DayManager.Instance.dayCount)
        {
            case 0:
                conversation.Execute("OldCat/Convo");
                break;
            default:
                conversation.Execute(beachCleanup.isCompleted?"OldCat/ConvoPos":"OldCat/ConvoNeg");
                break;
        }
    }
}
