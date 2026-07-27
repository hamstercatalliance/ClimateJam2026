using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectDatabase : MonoBehaviour
{
    public static ScriptableObjectDatabase Instance { get; private set; }
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
    [SerializeField] private List<ScriptableObject> allScriptableObjectRegistry; // every possible vanity item
    private Dictionary<string, ScriptableObject> scriptableObjectDictionary = new Dictionary<string, ScriptableObject>();
    void Start()
    {
        foreach (ScriptableObject obj in allScriptableObjectRegistry)
        {
            if (obj is GameItemSO gameItem)
            {
                scriptableObjectDictionary.Add(gameItem.itemID, obj);
            }
            else if (obj is QuestSO quest)
            {
                scriptableObjectDictionary.Add(quest.questID, obj);
            }
            else
            {
                // Handle other types of ScriptableObjects if needed
            }
        }
    }
    public ScriptableObject GetScriptableObjectByID(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        if (scriptableObjectDictionary.TryGetValue(id, out ScriptableObject obj))
        {
            return obj;
        }
        Debug.LogWarning($"ScriptableObject with ID '{id}' not found in the database.");
        return null;
    }
}
