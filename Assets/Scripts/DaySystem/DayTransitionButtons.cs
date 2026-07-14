using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class DayTransitionButtons : MonoBehaviour
{
    [SerializeField] private string HOME_SCENE = "Home";
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
        //can be directky called to exit without saving
        //EXIT TO MAIN MENU
    }
    public void Continue()
    {
        Debug.Log("Continuing...");
        //can be directly called to continue without saving
        SceneManager.LoadScene(HOME_SCENE); //NOT USING SCENE LOADER BECAUSE NO SCENE DATA DOES NEEDS TO BE SAVED ONTO GAMEDATA
    }
    private void Save()
    {
        Debug.Log("Saving...");
        //SET THE SAVE TO THE NEXT DAY
        //SAVE TO JSON
        //LOAD TO HOME SCENE
    }
}
