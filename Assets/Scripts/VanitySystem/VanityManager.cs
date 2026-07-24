using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class VanityManager : MonoBehaviour, IHasPersistentData
{
    public static VanityManager Instance { get; private set; } //singleton
    public bool DataSuccessfullyWritten { get; private set; }
    [SerializeField] private List<GameItemSO> allVanityItemsRegistry; // every possible vanity item
    private List<GameItemSO> ownedVanityItems = new List<GameItemSO>();

    public GameItemSO equipedVanityItem {get; private set;} // the currently equipped vanity item
    private GameItemSO selectedVanityItem; // the vanity item currently selected in the UI, but not necessarily equipped
    public EventHandler<OnVanityItemEquippedEventArgs> OnVanityItemEquipped;
    public class OnVanityItemEquippedEventArgs : EventArgs
    {
        public GameItemSO equippedItem;
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
    private void Start()
    {
        LoadGameData();
        SceneLoader.OnSceneTransition += SceneLoader_OnSceneTransition;
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= SceneLoader_OnSceneTransition;
    }
    private void SceneLoader_OnSceneTransition(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    public bool HasVanityItem(GameItemSO item)
    {
        return ownedVanityItems.Contains(item);
    }
    private void OnDisable()
    {
        selectedVanityItem = null;
    }
    public bool AddVanityItem(GameItemSO item)
    {
        if (HasVanityItem(item))
        {
            return false;
        }
        ownedVanityItems.Add(item);
        VanityUIManager.Instance.CreateVanityItemButton(item); // <-- add this
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
        if (equipedVanityItem != null)
        {
            GameData.Instance.EquippedVanityItemID = equipedVanityItem.itemID;
        }
        else
        {
            GameData.Instance.EquippedVanityItemID = null;
        }

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
                        AddVanityItem(match);
                    }
                    else
                    {
                        Debug.LogError("VanityManager: LoadGameData: Could not find GameItemSO with ID: " + id);
                    }
                }
            }
            string equippedID = GameData.Instance.EquippedVanityItemID;
            GameItemSO equippedItem = ScriptableObjectDatabase.Instance.GetScriptableObjectByID(equippedID) as GameItemSO;
            if (equippedItem != null)
            {
                equipedVanityItem = equippedItem;
                OnVanityItemEquipped?.Invoke(this, new OnVanityItemEquippedEventArgs 
                { 
                    equippedItem = equippedItem 
                });
                VanityPlayerPreview.Instance.SetStayVisible(true);
                VanityPlayerPreview.Instance.ShowEquippedOrNothing();
            }
            else
            {
                Debug.Log("VanityManager: LoadGameData: No equipped vanity item found in saved data.");
            }
        }
        else
        {
            //new game
            ownedVanityItems = new List<GameItemSO>();
        }
    }
    public void EquipVanityItem()
    {
        equipedVanityItem = selectedVanityItem;
        OnVanityItemEquipped?.Invoke(this, new OnVanityItemEquippedEventArgs 
        { 
            equippedItem = selectedVanityItem
        });
    }
    public void UnequipVanityItem()
    {
        OnVanityItemEquipped?.Invoke(this, new OnVanityItemEquippedEventArgs 
        { 
            equippedItem = null 
        });
        ClearSelectedVanityItem();
        VanityPlayerPreview.Instance.SetStayVisible(false);
        VanityPlayerPreview.Instance.ClearPreview();
        equipedVanityItem = null;
    }
    public void SetSelectedVanityItem(GameItemSO item)
    {
        if (item == null || !HasVanityItem(item) || item.isVanityItem == false)
        {
            Debug.LogError("VanityManager: SetSelectedVanityItem: Cannot select item that is not owned or is not a vanity item.");
            return;
        }
        selectedVanityItem = item;
    }
    public GameItemSO GetSelectedVanityItem()
    {
        return selectedVanityItem;
    }
    public void ClearSelectedVanityItem()
    {
        selectedVanityItem = null;
    }
}
