using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneInteractable : InteractableObject
{
    [SerializeField] private string dialogueName;
    protected override void OnTalk()
    {
        conversation.Execute(dialogueName);
    }

    protected override void Init()
    {

    }
}
