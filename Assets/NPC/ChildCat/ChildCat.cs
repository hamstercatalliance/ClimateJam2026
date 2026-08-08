//csharp Assets\NPC\ChildCat\ChildCat.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCat : NonPlayableCharacter
{
    public bool questCompleted;
    public bool questStarted;
    public bool hasInteracted;
    public bool positiveInteraction;
    private int dayInteracted;

    protected override void OnTalk()
    {
        // First-time interaction: record and return
        if (!hasInteracted)
        {
            hasInteracted = true;
            dayInteracted = DayManager.Instance.dayCount;
            conversation.Execute("ChildCat/Convo1");
            return;
        }

        if (!questStarted)
        {
            bool isLaterDay = DayManager.Instance.dayCount > dayInteracted;
            if (isLaterDay)
            {
                ExecuteDialogue("QuestStart");
            }
            else
            {
                ExecuteDialogue("1");
            }
            return;
        }

        // If quest started, handle completed vs in-progress responses
        if (questCompleted)
        {
            ExecuteDialogue("QuestPost");
        }
        else
        {
            GiveInitialQuestResponse();
        }
    }

    private void ExecuteDialogue(string name)
    {
        string qual = positiveInteraction ? "Pos" : "Neg";
        conversation.Execute(string.Format("ChildCat/Convo{0}{1}", name, qual));
    }

    public void setInteraction (bool interaction)
    {
        hasInteracted = true;
        positiveInteraction = interaction;
    }


    void GiveInitialQuestResponse()
    {
        CoconutWaterQuest coconutWaterQuest = FindFirstObjectByType<CoconutWaterQuest>();
        ColaQuest colaQuest = FindFirstObjectByType<ColaQuest>();

        if (coconutWaterQuest.isInitiated && coconutWaterQuest.CheckCocounutWater())
        {
            questCompleted = true;
            ExecuteDialogue("QuestEnd");
        }
        else if (colaQuest.isInitiated && colaQuest.CheckColas())
        {
            questCompleted = true;
            ExecuteDialogue("QuestEnd");
        } else
        {
            ExecuteDialogue("Quest");
        }
    }

}