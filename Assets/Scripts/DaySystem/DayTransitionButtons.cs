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
    [SerializeField] private float countdownDuration = 1f;
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
        StartCoroutine(ShowCountdownAndContinue());
        SceneManager.LoadScene(HOME_SCENE); //NOT USING SCENE LOADER BECAUSE NO SCENE DATA DOES NEEDS TO BE SAVED ONTO GAMEDATA
    }
    private void Save()
    {
        Debug.Log("Saving...");
        DataPersistenceManager.Instance.SavePlayerData();//SAVE TO JSON
    }
    
    private IEnumerator ShowCountdownAndContinue()
    {
        dayCountdown.ShowCountdown();
        yield return new WaitForSeconds(countdownDuration);
        Continue();
    }
}
