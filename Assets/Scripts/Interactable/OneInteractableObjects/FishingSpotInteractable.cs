using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpotInteractable : OneInteractable
{
    private FishingCarp fishingQuest;

    public string FishingNoRodDialoguePath = "OneInteract/FishingQuest/FishingNoRod";
    public string FishingNoBaitDialoguePath = "OneInteract/FishingQuest/FishingNoBait";
    public string FishingCarpDialoguePath = "OneInteract/FishingQuest/FishingCarp";
    public string FishingSturgeonDialoguePath = "OneInteract/FishingQuest/FishingSturgeon";
    public string FishingCatfishDialoguePath = "OneInteract/FishingQuest/FishingCatfish";


    protected override void OnTalk()
    {
        fishingQuest = FindObjectOfType<FishingCarp>();
        Debug.Log(fishingQuest.HasBait());
        bool hasFishingRod = fishingQuest.HasFishingRod();
        if (!hasFishingRod)
        {
            Debug.Log("Player does not have a fishing rod.");
            conversation.Execute(FishingNoRodDialoguePath);
            return;
        }
        else if (!fishingQuest.HasBait())
        {
            Debug.Log("Player does not have bait.");
            
            conversation.Execute(FishingNoBaitDialoguePath);
            return;
        }
        else
        {
            Debug.Log("Player has a fishing rod and bait. Proceeding to fish.");
            string dialogueName; 
            GameItemSO selectedFish = fishingQuest.Fish();
            if (selectedFish.itemID == "item.carp")
            {
                dialogueName = FishingCarpDialoguePath;
            }
            else if (selectedFish.itemID == "item.sturgeon")
            {
                dialogueName = FishingSturgeonDialoguePath;
            }
            else //if (selectedFish.itemID == "item.catfish")
            {
                dialogueName = FishingCatfishDialoguePath;
            }
            conversation.Execute(dialogueName);
        }
    }
}
