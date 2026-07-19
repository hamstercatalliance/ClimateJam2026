using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPlayableCharacter : InteractableObject
{
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    protected override void Init() {
        NonPlayableCharacterSO nonPlayableCharacterSO = scriptableInteractable as NonPlayableCharacterSO;
        transform.position = nonPlayableCharacterSO.location;
    }
}
