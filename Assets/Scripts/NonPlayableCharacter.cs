using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPlayableCharacter : InteractableObject
{


    protected override void Init() {
        NonPlayableCharacterSO nonPlayableCharacterSO = scriptableInteractable as NonPlayableCharacterSO;
        transform.position = nonPlayableCharacterSO.location;
    }
}
