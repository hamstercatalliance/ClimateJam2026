using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishercat : NonPlayableCharacter
{
    public MerchantStore fishingStore;
    protected override void OnTalk()
    {
        fishingStore.EnterStore();
    }
}
