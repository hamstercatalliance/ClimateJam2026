using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CurrencyManager;

public class SympathyPointsManager : DialogueSignalHandler, IHasPersistentData
{
    public static SympathyPointsManager Instance { get; private set; }
    public int SympathyPoints { get; private set; }

    private int goodEndingThreshold = 900;
    public bool HasReachedGoodEndingThreshold()
    {
        Debug.Log("Checking if good ending threshold is reached. Current points: " + SympathyPoints + ", Threshold: " + goodEndingThreshold);
        return SympathyPoints >= goodEndingThreshold;
    }

    private void Start()
    {
        LoadGameData();
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
    }

    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        if (e is AddPointsDialogueSignal)
        {
            AddPointsDialogueSignal addPointsSignal = e as AddPointsDialogueSignal;
            AddSympathyPoints(addPointsSignal.points);
            Debug.Log("Sympathy points changed by " + addPointsSignal.points + ". Total sympathy points: " + SympathyPoints);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool DataSuccessfullyWritten { get; private set; }


    public void AddSympathyPoints(int addedPoints) {
        Debug.Log("Adding sympathy points: " + addedPoints + ". Current sympathy points: " + SympathyPoints);
        SympathyPoints += addedPoints;
    }




    public void WriteToGameData() {
        GameData.Instance.SympathyPoints = SympathyPoints;
        DataSuccessfullyWritten = true;
        Debug.Log("Sympathy points written to GameData: " + SympathyPoints);
    }

    public void LoadGameData()
    {
        Debug.Log("Loading" + GameData.Instance.SympathyPoints + " sympathy points from GameData.");
        SympathyPoints = GameData.Instance.SympathyPoints;
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            SympathyPoints = GameData.Instance.SympathyPoints;
        }
    }
    private void OnSceneTransitionHandler(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }

}
