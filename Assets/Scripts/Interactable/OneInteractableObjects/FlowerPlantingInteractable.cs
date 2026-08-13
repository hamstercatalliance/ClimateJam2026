using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlantingInteractable : InteractableObject
{
    protected override void OnTalk()
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
