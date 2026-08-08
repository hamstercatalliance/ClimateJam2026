using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishercat : NonPlayableCharacter
{
    public MerchantStore fishingStore;
    bool hasInteracted = false;
    protected override void OnTalk()
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            conversation.Execute("Fishercat/Convo1");
        }
        else
        {
            fishingStore.EnterStore();
        }
    }
}
