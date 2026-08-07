//csharp Assets\NPC\ChildCat\ChildCat.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCat : NonPlayableCharacter
{
    bool questCompleted;
    bool questStarted;
    bool hasInteracted;
    public bool positiveInteraction;
    int dayInteracted;

    protected override void OnTalk()
    {
        // First-time interaction: record and return
        if (!hasInteracted)
        {
            hasInteracted = true;
            dayInteracted = DayManager.Instance.dayCount;
            return;
        }

        // If the quest hasn't started, decide between giving the quest (later day)
        // or giving a response (same day), using a small helper to keep branches clear.
        if (!questStarted)
        {
            bool isLaterDay = DayManager.Instance.dayCount > dayInteracted;
            if (isLaterDay)
            {
                GiveQuest(positiveInteraction);
            }
            else
            {
                GiveSameDayResponse(positiveInteraction);
            }
            return;
        }

        // If quest started, handle completed vs in-progress responses
        if (questCompleted)
        {
            GiveCompletedQuestResponse();
        }
        else
        {
            GiveInitialQuestResponse();
        }
    }

    // Helper methods to encapsulate response/quest logic for readability.
    // Replace comment blocks with actual implementation (dialogue/quest assignment).

    void GiveQuest(bool positive)
    {
        if (positive)
        {
            // Give positive quest
        }
        else
        {
            // Give negative quest
        }
    }

    void GiveSameDayResponse(bool positive)
    {
        if (positive)
        {
            // Give positive response
        }
        else
        {
            // Give negative response
        }
    }

    void GiveCompletedQuestResponse()
    {
        // Give completed quest response
    }

    void GiveInitialQuestResponse()
    {
        // Give initial quest response
    }
}