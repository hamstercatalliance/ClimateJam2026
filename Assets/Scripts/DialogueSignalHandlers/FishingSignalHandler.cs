using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSignalHandler : DialogueSignalHandler
{
    public static event EventHandler OnRodCasted;
    public static event EventHandler OnFishHooked;
    public static event EventHandler OnFishReeled;
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
            case "fishing_start":
                fishingQuest.ShowFishingRod();
                OnRodCasted?.Invoke(this, EventArgs.Empty);
                break;
            case "wait_for_fish":
                OnFishHooked?.Invoke(this, EventArgs.Empty);
                break;
            case "reel_carp":
                fishingQuest.HideFishingRod();
                OnFishReeled?.Invoke(this, EventArgs.Empty);
                fishingQuest.ReelFish(fishingQuest.carp);
                break;
            case "reel_sturgeon":
                fishingQuest.HideFishingRod();
                OnFishReeled?.Invoke(this, EventArgs.Empty);
                fishingQuest.ReelFish(fishingQuest.sturgeon);
                break;
            case "reel_catfish":
                fishingQuest.HideFishingRod();
                OnFishReeled?.Invoke(this, EventArgs.Empty);
                fishingQuest.ReelFish(fishingQuest.catfish);
                break;
        }
    }
}
