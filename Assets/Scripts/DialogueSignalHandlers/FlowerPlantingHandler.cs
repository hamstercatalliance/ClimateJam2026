using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPlantingHandler : DialogueSignalHandler
{
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;
        if (dialogueSignal.signal == "flower_planting_confirm")
        {
            FlowerPlanting flowerPlanting = FindObjectOfType<FlowerPlanting>();
            flowerPlanting.PlantFlowers();
        }
    }
}
