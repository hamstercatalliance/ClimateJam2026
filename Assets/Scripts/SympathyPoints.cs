using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SympathyPoints : MonoBehaviour, IHasPersistentData
{
    public int sympathyPoints { get; private set; }

    public bool DataSuccessfullyWritten { get; private set; }

    public SympathyPoints Instance { get; private set; }


    void Awake() { 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    public void AddSympathyPoints(int amount)
    {
        sympathyPoints += amount;
    }

    public void SubtractSympathyPoints(int amount)
    {
        sympathyPoints -= amount;
    }

    public void WriteToGameData()
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.SympathyPoints = sympathyPoints;
            DataSuccessfullyWritten = true;
        }
    }

    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            sympathyPoints = GameData.Instance.SympathyPoints ?? 0;
        }
    }
}