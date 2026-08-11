using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamsphere : NonPlayableCharacter
{
    protected override void OnTalk()
    {
        switch (DayManager.Instance.dayCount)
        {
            case 0:
                conversation.Execute("Hamsphere/Convo1");
                break;
            case 1:
                conversation.Execute("Hamsphere/Convo1");
                break;
            case 2:
                conversation.Execute("Hamsphere/Convo3");
                break;
        }
    }
}
