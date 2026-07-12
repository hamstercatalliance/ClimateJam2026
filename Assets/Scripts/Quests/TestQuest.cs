using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
public class TestQuest : Quest
{
    //Goal is to pick up 3 carp
    private int carpCount = 0;
    private int carpGoal = 3;
    protected override void Start()
    {
        base.Start();
        Player.Instance.OnPickup += Player_OnPickup;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Player.Instance.OnPickup -= Player_OnPickup;
    }
    private void Player_OnPickup(object sender, Player.OnPickupEventArgs e)
    {
        if (isInitiated && !isCompleted)
        {
            if (e.gameItemSO.itemName == "Carp")
            {
                carpCount++;
                Debug.Log("Carp picked up: " + carpCount);
                if (carpCount >= carpGoal)
                {
                    CompleteQuest();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the quest area");
            InitiateQuest();
        }
    }

    public override void InitiateQuest()
    {
        base.InitiateQuest();
        Debug.Log("TestQuest has been initiated");
    }
    public override void CompleteQuest()
    {
        base.CompleteQuest();
        Debug.Log("TestQuest has been completed");
    }
    public override void WriteToGameData()
    {
        GameData.Instance.TestQuestProgress = carpCount;
        base.WriteToGameData();
    }
    public override void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            carpCount = GameData.Instance.TestQuestProgress;
            if (carpCount >= carpGoal)
            {
                isCompleted = true;
            }
            else if (carpCount > 0)
            {
                isInitiated = true;
            }
        }
        else
        {
            carpCount = 0;
            isInitiated = false;
            isCompleted = false;
        }
    }
}