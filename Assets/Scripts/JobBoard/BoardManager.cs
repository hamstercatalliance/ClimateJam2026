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
    [SerializeField] private GameInput gameInput;
    public static bool jobBoardActive { get; private set; } = false;
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
        gameInput.OnMenuAction += GameInput_OnMenuAction;
    }

    // will loop through quests, check if initiated, adds an additional quest entry to the board
    public void RenderBoard() {
        int day = DayManager.Instance.dayCount;
        foreach (BoardEntryManager quest in quests) {
            bool renderQuest = !quest.getQuest.isInitiated && quest.getDay <= day;
            quest.gameObject.SetActive(renderQuest);
        }
    }

    private void GameInput_OnMenuAction(object sender, System.EventArgs e) {
        if (jobBoardActive)
        {
            CloseBoard();
        }
    }
    private void OnDestroy()
    {
        gameInput.OnMenuAction -= GameInput_OnMenuAction;
    }
    public void OpenBoard()
    {
        board.gameObject.SetActive(true);
        RenderBoard();
        jobBoardActive = true;
    }

    public void CloseBoard()
    {

        board.gameObject.SetActive(false);
        StartCoroutine(wait(0.1f));
    }

    private IEnumerator wait (float sec) { 
        yield return new WaitForSeconds(sec);
        jobBoardActive = false;
    }
}
