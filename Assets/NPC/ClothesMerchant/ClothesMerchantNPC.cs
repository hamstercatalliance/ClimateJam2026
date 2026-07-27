using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesMerchantNPC : NonPlayableCharacter
{
    private bool interacted = false;
    public MerchantStore store;
    protected override void OnTalk()
    {
        if (interacted) { 
            store.EnterStore();
        } else
        {
            interacted = true;
            conversation.Execute("ClothesMerchant/Convo1");
        }
    }
}
