using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TestQuest : Quest
{
    //Goal is to pick up 3 carp
    private int carpCount = 0;
    private int carpGoal = 3;
    private void Start()
    {
        Player.Instance.OnPickup += Player_OnPickup;
    }
    private void OnDestroy()
    {
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
}