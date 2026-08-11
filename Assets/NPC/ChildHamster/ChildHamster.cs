using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildHamster : NonPlayableCharacter
{
    public bool positiveConvo;
    public bool hasInteracted;
    public int dayInteracted = 3;
    protected override void OnTalk() {
        if (dayInteracted < DayManager.Instance.dayCount) {
            firstDayInteraction();
        } else
        {
            laterInteraction();
        }
           
     
    }
    public void SetInteraction(bool qual)
    {
        positiveConvo = qual;
        hasInteracted = true;
        dayInteracted = DayManager.Instance.dayCount;
    }

    private void firstDayInteraction ()
    {
        if (!hasInteracted)
        {
            conversation.Execute("ChildHamster/Convo1");
        }
        else
        {
            if (positiveConvo)
            {
                conversation.Execute("ChildHamster/ConvoPos");
            }
            else
            {
                conversation.Execute("ChildHamster/ConvoNeg");
            }
        }
    }

    private void laterInteraction()
    {
        if (positiveConvo)
        {
            conversation.Execute("ChildHamster/Convo2Pos");
        }
        else
        {
            conversation.Execute("ChildHamster/Convo2Neg");
        }
    }

}
