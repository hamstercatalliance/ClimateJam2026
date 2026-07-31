using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class DayTransitionButtons : MonoBehaviour
{
    [SerializeField] private string HOME_SCENE = "Home";
    [SerializeField] private string MENU_SCENE = "Menu";
    [SerializeField] private DayCountdown dayCountdown;
    public void SaveAndContinue()
    {
        Save();
        Continue();
    }
    public void SaveAndExit()
    {
        Save();
        Exit();
    }
    public void Exit()
    {
        Debug.Log("Exiting...");
        SceneManager.LoadScene(MENU_SCENE);
    }
    public void Continue()
    {
        Debug.Log("Continuing...");
        SceneManager.LoadScene(HOME_SCENE);
        // StartCoroutine(ContinueRoutine());
    }
    private void Save()
    {
        Debug.Log("Saving...");
        DataPersistenceManager.Instance.SavePlayerData();//SAVE TO JSON
    }
}
