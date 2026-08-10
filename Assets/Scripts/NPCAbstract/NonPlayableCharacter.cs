using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPlayableCharacter : InteractableObject
{
    [Header("NPC orientation & position:\neach index represents a correspondingday")]
    [SerializeField] private Vector3[] spawnPoints;
    [SerializeField] private bool[] isFacingRight; 

    // private void Update()
    // {
    //     transform.rotation = Quaternion.Euler(0, 0, 0);
    // }

    protected override void Init() {
        NonPlayableCharacterSO nonPlayableCharacterSO = scriptableInteractable as NonPlayableCharacterSO;

        //pulling straight from data in case it's not initialized yet in DayManager
        int curDay = GameData.Instance.DayManagerDayCount;

        if (spawnPoints.Length == 0)
        {
            transform.localPosition = nonPlayableCharacterSO.location;
        }
        else
        {
            if (curDay >= spawnPoints.Length)
            {
                Debug.LogWarning("No spawn point available for day " + curDay);
                //if no spawn point is available for this day, use the default location
                transform.localPosition = nonPlayableCharacterSO.location;
            }
            else
            {
                Debug.Log("Using spawn point for day " + curDay + ": " + spawnPoints[curDay]);
                bool isFinalDayBadEnd = FinalDayEndingManager.Instance.IsFinalDay() && GameData.Instance.SympathyPoints < 50;
                if (isFinalDayBadEnd)
                {
                    //dont move the NPC if its the bad end day
                    transform.localPosition = nonPlayableCharacterSO.location; //just default
                    return;
                }
                Debug.Log("Moving NPC to spawn point for day " + curDay);
                transform.localPosition = spawnPoints[curDay];
            }
        }
    
        //npcs look left on default
        if (isFacingRight.Length == 0)
        {
            transform.forward = new Vector3(0, 0, 1);
        }
        else
        {
            if (curDay >= isFacingRight.Length)
            {
                // If no facing direction is available for this day, default to looking left
                transform.forward = new Vector3(0, 0, 1);
            }
            else
            {
                if (isFacingRight[curDay])
                {
                    transform.forward = new Vector3(0, 0, -1);
                }
                else
                {
                    transform.forward = new Vector3(0, 0, 1);
                }
            }
        }
    }
}
