using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlantingInteractable : OneInteractable
{
    public void DisableInteraction()
    {
        InteractableObject interactableObject = GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            interactableObject.enabled = false;
        }
    }
}
