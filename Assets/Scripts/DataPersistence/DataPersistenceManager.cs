using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    private const string SaveFileName = "gamedata.json";
    public static DataPersistenceManager Instance { get; private set; }
    public string SavePath
    {
        get { return Path.Combine(Application.persistentDataPath, SaveFileName); }
        private set { }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        LoadPlayerData();
        if (!File.Exists(SavePath))
        {
            SavePlayerData();
        }
    }
    public void SavePlayerData()
    {
        //(unencrypted)
        string json = JsonUtility.ToJson(GameData.Instance.GetSaveData(), true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Game data saved to: " + SavePath);
    }
    public void LoadPlayerData()
    {
        if (!File.Exists(SavePath))
        {
            return;
        }

        string json = File.ReadAllText(SavePath);
        GameData.SaveData saveData = JsonUtility.FromJson<GameData.SaveData>(json);
        GameData.Instance.LoadFromSaveData(saveData);
    }
    public void ClearGame()
    {
        DeletePlayerData();
        GameData.Instance.ClearData();
        SavePlayerData();
    }
    private void DeletePlayerData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
    }
}
