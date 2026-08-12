using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlantingInteractable : InteractableObject
{
    bool disableInteraction;
    public void DisableInteraction()
    {
        InteractableObject interactableObject = GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            ZoneExited();
            interactableObject.enabled = false;
            disableInteraction = true;

        }
    }

    protected override void OnTalk()
    {
        if (!disableInteraction)
        {
            FlowerPlanting flowerPlanting = FindObjectOfType<FlowerPlanting>();
            if (flowerPlanting.CheckFlowerSeeds())
            {
                conversation.Execute("OneInteract/FlowerPlantingSpot");
            }
            else
            {
                conversation.Execute("OneInteract/FlowerPlantingSpotDeny");
            }
        }
    }
}
