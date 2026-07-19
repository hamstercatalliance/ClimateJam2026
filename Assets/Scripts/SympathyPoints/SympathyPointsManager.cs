using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CurrencyManager;

public class SympathyPointsManager : DialogueSignalHandler, IHasPersistentData
{
    public static SympathyPointsManager Instance { get; private set; }
    public int SympathyPoints { get; private set; }


    private void Start()
    {
        LoadGameData();
    }


    protected override void HandleDialogueSignal(object sender, EventArgs e)
    {
        if (e is AddPointsDialogueSignal)
        {
            AddPointsDialogueSignal addPointsSignal = e as AddPointsDialogueSignal;
            addSympathyPoints(addPointsSignal.points);
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


    public void addSympathyPoints(int addedPoints) {
        addedPoints += SympathyPoints;
    }




    public void WriteToGameData() {
        GameData.Instance.SympathyPoints = SympathyPoints;
        DataSuccessfullyWritten = true;
    }

    public void LoadGameData()
    {
        SympathyPoints = GameData.Instance.SympathyPoints;
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            SympathyPoints = GameData.Instance.SympathyPoints;
        }
    }


}
