using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHamster : NonPlayableCharacter
{
    protected override void OnTalk() {
        switch (DayManager.Instance.dayCount) {
            case 0:
                conversation.Execute("ChildHamster/Convo1");
                break;
            //case 1:
            //    conversation.Execute("ChildHamster/Convo2");
            //    break;
            default:
                conversation.Execute("ChildHamster/Convo1");
                break;
        }
    }

}
