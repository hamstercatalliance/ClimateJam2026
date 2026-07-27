using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoardInteractable : InteractableObject
{
    [SerializeField] GameObject jobBoard;
    protected override void OnTalk() {
        if (!BoardManager.jobBoardActive)
        {
            //Debug.Log("ONTALK Opening board");
            BoardManager.Instance.OpenBoard();
        } 
    }
}
