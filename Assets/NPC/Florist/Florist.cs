using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Florest : NonPlayableCharacter
{
    protected override void OnTalk()
    {
    switch (GameData.Instance.DayManagerDayCount)
        {
            case 0:
                conversation.Execute("Florist/Convo1");
                break;
        }
    }
}
