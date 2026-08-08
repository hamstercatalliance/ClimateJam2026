using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System;
public class BoardManager : MonoBehaviour
{
    public event EventHandler OnBoardOpened;
    public event EventHandler OnBoardClosed;
    private BoardEntryManager[] questEntries;
    [SerializeField] Transform board;
    public static BoardManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this; 
    }
    [SerializeField] private GameInput gameInput;
    public static bool jobBoardActive { get; private set; } = false;
    private void Start()
    {
        //gameObject.SetActive(false);
        gameInput.OnMenuAction += GameInput_OnMenuAction;
    }
    private void OnDestroy()
    {
        gameInput.OnMenuAction -= GameInput_OnMenuAction;
    }
    // will loop through quests, check if initiated, adds an additional quest entry to the board
    public void RenderBoard() {
        questEntries = GetComponentsInChildren<BoardEntryManager>();
        int day = DayManager.Instance.dayCount;
        foreach (BoardEntryManager questEntry in questEntries) {
            Quest quest = questEntry.GetQuest;
            if (quest != null)
            {
                bool renderQuest = (!quest.isInitiated) && (questEntry.GetDay <= day);
                questEntry.gameObject.SetActive(renderQuest);
            }
        }
    }

    private void GameInput_OnMenuAction(object sender, System.EventArgs e) {
        if (jobBoardActive)
        {
            CloseBoard();
        }
    }
    public void OpenBoard()
    {
        board.gameObject.SetActive(true);
        if (OpenJobBoard.Instance.isInitiated && OpenJobBoard.Instance.isCompleted == false)
        {
            //Debug.Log()
            OpenJobBoard.Instance.setCondition();
        }
        RenderBoard();
        jobBoardActive = true;

        OnBoardOpened?.Invoke(this, EventArgs.Empty);
    }

    public void CloseBoard()
    {
        OnBoardClosed?.Invoke(this, EventArgs.Empty);
        
        board.gameObject.SetActive(false);
        StartCoroutine(wait(0.1f));
    }

    private IEnumerator wait (float sec) { 
        yield return new WaitForSeconds(sec);
        jobBoardActive = false;
    }
}
