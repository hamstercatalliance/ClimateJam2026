using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSignalHandler : DialogueSignalHandler
{
    private FishingCarp fishingQuest;
    private Conversation conversation;
    private FishingSpotInteractable fishingSpotInteractable;
    private void Start()
    {
        fishingQuest = FindFirstObjectByType<FishingCarp>();
        conversation = FindAnyObjectByType<Conversation>();
        fishingSpotInteractable = FindAnyObjectByType<FishingSpotInteractable>();
    }
    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        DialogueSignal dialogueSignal = e as DialogueSignal;
        string signalString = dialogueSignal.signal;
        Debug.Log($"FishingSignalHandler received signal: {signalString}");
        switch (signalString)
        {
            case "carp_caught":
                fishingQuest.ReelFish(fishingQuest.carp);
                break;
            case "sturgeon_caught":
                fishingQuest.ReelFish(fishingQuest.sturgeon);
                break;
            case "catfish_caught":
                fishingQuest.ReelFish(fishingQuest.catfish);
                break;
        }
    }
}
