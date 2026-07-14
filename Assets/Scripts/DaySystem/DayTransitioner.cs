using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DayTransitioner : MonoBehaviour
{
    //THIS IS FOR GOING FROM GAMEPLAY TO SAVE SCENE
    [SerializeField] private SceneLoader dayTransitionSceneLoader;
    private void Start()
    {
        DayManager.Instance.OnDayEnd += DayManager_OnDayEnd;
    }
    private void OnDestroy()
    {
        DayManager.Instance.OnDayEnd -= DayManager_OnDayEnd;
    }
    private void DayManager_OnDayEnd(object sender, EventArgs e)
    {
        dayTransitionSceneLoader.LoadSceneRoutine();
    }
}
