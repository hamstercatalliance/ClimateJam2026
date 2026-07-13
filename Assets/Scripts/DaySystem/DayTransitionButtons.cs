using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DayTransitionButtons : MonoBehaviour
{
    public static event EventHandler OnDayContinuePressed;
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
        //can be directky called to exit without saving
        //EXIT TO MAIN MENU
    }
    public void Continue()
    {
        //can be directly called to continue without saving
        OnDayContinuePressed?.Invoke(this, EventArgs.Empty);
    }
    private void Save()
    {
        //SET THE SAVE TO THE NEXT DAY
        //DAY MANAGER 
        //SAVE TO JSON
    }
}
