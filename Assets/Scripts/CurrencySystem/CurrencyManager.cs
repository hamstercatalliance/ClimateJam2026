using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CurrencyManager : MonoBehaviour, IHasPersistentData
{
    public static CurrencyManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private int currencyAmount = 0;
    public bool DataSuccessfullyWritten { get; private set;}
    public event EventHandler<OnCurrencyChangedEventArgs> OnCurrencyChanged;
    public class OnCurrencyChangedEventArgs : EventArgs
    {
        public int newCurrencyAmount;
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        LoadGameData();
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    private void OnSceneTransitionHandler(object sender, System.EventArgs e)
    {
        WriteToGameData();
    }
    public void AddCurrency(int amount)
    {
        currencyAmount += amount;
        Debug.Log("Added " + amount + " currency. Total currency: " + currencyAmount);
        OnCurrencyChanged?.Invoke(this, new OnCurrencyChangedEventArgs 
        { 
            newCurrencyAmount = currencyAmount 
        });
    }
    public void RemoveCurrency(int amount)
    {
        currencyAmount -= amount;
        Debug.Log("Removed " + amount + " currency. Total currency: " + currencyAmount);
        OnCurrencyChanged?.Invoke(this, new OnCurrencyChangedEventArgs 
        { 
            newCurrencyAmount = currencyAmount 
        });
    }
    public int GetCurrencyAmount()
    {
        return currencyAmount;
    }
    public void WriteToGameData()
    {
        GameData.Instance.currencyAmount = currencyAmount;
        DataSuccessfullyWritten = true;
        Debug.Log("Currency amount written to game data: " + currencyAmount);
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            currencyAmount = GameData.Instance.currencyAmount;
            Debug.Log("Currency amount loaded from game data: " + currencyAmount);
            OnCurrencyChanged?.Invoke(this, new OnCurrencyChangedEventArgs 
            { 
                newCurrencyAmount = currencyAmount 
            });
        }
        else
        {
            currencyAmount = 250; // Default starting currency amount
            OnCurrencyChanged?.Invoke(this, new OnCurrencyChangedEventArgs 
            { 
                newCurrencyAmount = currencyAmount 
            });
            Debug.Log("No game data found. Starting with default currency amount: " + currencyAmount);
        }
    }
}
