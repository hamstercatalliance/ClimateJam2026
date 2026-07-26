using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{

    private BoardEntryManager[] quests;
    [SerializeField] Transform board;
    public static BoardManager Instance;
    private void Start()
    {
        //gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this; // TODO: Fix signleton check
        quests = GetComponentsInChildren<BoardEntryManager>();
        //board = transform.Find("board");

    }

    // will loop through quests, check if initiated, adds an additional quest entry to the board
    public void RenderBoard() {
        int day = DayManager.Instance.dayCount;
        foreach (BoardEntryManager quest in quests) {
            bool renderQuest = !quest.getQuest.isInitiated && quest.getDay <= day;
            quest.gameObject.SetActive(renderQuest);
        }
    }
}
