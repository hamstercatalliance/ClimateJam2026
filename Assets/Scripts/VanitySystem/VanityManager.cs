using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanityManager : MonoBehaviour, IHasPersistentData
{
    public static VanityManager Instance { get; private set; } //singleton
    public bool DataSuccessfullyWritten { get; private set; }
    [SerializeField] private List<GameItemSO> allVanityItemsRegistry; // every possible vanity item
    private List<GameItemSO> ownedVanityItems = new List<GameItemSO>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        LoadGameData();
        SceneLoader.OnSceneTransition += SceneLoader_OnSceneTransition;
    }
    private void SceneLoader_OnSceneTransition(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    public bool HasVanityItem(GameItemSO item)
    {
        return ownedVanityItems.Contains(item);
    }
    public bool AddVanityItem(GameItemSO item)
    {
        if (HasVanityItem(item))
        {
            return false;
        }
        ownedVanityItems.Add(item);
        return true;
    }
    public void WriteToGameData()
    {
        List<string> ids = new List<string>();
        foreach (GameItemSO item in ownedVanityItems)
        {
            ids.Add(item.itemID);
        }
        GameData.Instance.OwnedVanityItemIDs = ids;
        DataSuccessfullyWritten = true;
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            List<string> savedIDs = GameData.Instance.OwnedVanityItemIDs;
            if (savedIDs != null)
            {
                foreach (string id in savedIDs)
                {
                    GameItemSO match = ScriptableObjectDatabase.Instance.GetScriptableObjectByID(id) as GameItemSO; 
                    if (match != null)
                    {
                        ownedVanityItems.Add(match);
                    }
                    else
                    {
                        Debug.LogError("VanityManager: LoadGameData: Could not find GameItemSO with ID: " + id);
                    }
                }
            }
        }
    }
}
