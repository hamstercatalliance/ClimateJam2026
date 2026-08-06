using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachCleanup : Quest
{
    [SerializeField] private GameObject trash;
    [SerializeField] private GameItemSO[] trashItems;
    private int trashCount = 0;
    private int trashGoal = 12;
    private HashSet<GameItemSO> trashItemSet;
    protected override void Start()
    {
        trashItemSet = new HashSet<GameItemSO>(trashItems);
        base.Start();
        Player.Instance.OnPickup += Player_OnPickup;

        StartCoroutine(LateStart());
    }
    private IEnumerator LateStart()
    {
        yield return null; // wait one frame

        if (trash != null)
        {
            if (isInitiated && !isCompleted)
            {
                trash.SetActive(true);
            }
            else
            {
                trash.SetActive(false);
            }
        }
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
            if (IsTrashItem(e.gameItemSO))
            {
                Debug.Log("Trash picked up: " + trashCount);
                trashCount++;
                if (trashCount >= trashGoal)
                {
                    Debug.Log("Beach cleanup quest completed!");
                    CompleteQuest();
                }
            }
        }
    }
    private bool IsTrashItem(GameItemSO item)
    {
        return trashItemSet.Contains(item);
    }

    public override void InitiateQuest()
    {
        base.InitiateQuest();
        if (trash != null)
        {
            trash.SetActive(true);
        }
    }
    public override void CompleteQuest()
    {
        base.CompleteQuest();
        if (trash != null)
        {
            trash.SetActive(false);
        }
    }
    public override void WriteToGameData()
    {
        GameData.Instance.BeachCleanupProgress = trashCount;
        base.WriteToGameData();
    }
    public override void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            trashCount = GameData.Instance.BeachCleanupProgress;
        }
        else
        {
            trashCount = 0;
        }
    }
}
