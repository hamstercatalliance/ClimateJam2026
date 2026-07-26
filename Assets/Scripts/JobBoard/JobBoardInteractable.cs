using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoardInteractable : InteractableObject
{
    [SerializeField] GameObject jobBoard;
    public static bool jobBoardActive = false;
    protected override void OnTalk() {
        if (!jobBoardActive)
        {
            //Debug.Log("ONTALK Opening board");
            OpenBoard();
        } else
        {
            //Debug.Log("ONTALK Closing board");
            CloseBoard();
        }
    }
    private void OpenBoard() {
        jobBoard.SetActive(true);
        jobBoard.GetComponent<BoardManager>().RenderBoard();
        jobBoardActive = true;
    }

    private void CloseBoard() { 
        jobBoard.SetActive(false);
        jobBoardActive = false;
    }
}
