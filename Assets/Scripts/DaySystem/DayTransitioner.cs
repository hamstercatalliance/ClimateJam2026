using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DayTransitioner : MonoBehaviour
{
    [SerializeField] private GameObject dayTransitionUI; //entire panel show immediately
    [SerializeField] private GameObject dayTransitionText; //text fade in
    [SerializeField] private GameObject buttonContainer; //button fade in
    [SerializeField] private float textFadeInDuration = 2f;
    [SerializeField] private float buttonFadeInDuration = 2f;
    private void Start()
    {
        dayTransitionUI.SetActive(false);
        DayManager.Instance.OnDayEnd += DayManager_OnDayEnd;
        DayTransitionButtons.OnDayContinuePressed += DayTransitionButtons_OnDayContinuePressed;
    }
    private void DayTransitionButtons_OnDayContinuePressed(object sender, EventArgs e)
    {
        Debug.Log(dayTransitionUI.name + " - Day continue button pressed.");
        dayTransitionUI.SetActive(false);
        DayManager.Instance.SetState(DayManager.State.Sunrising);
        Debug.Log("Day transition complete. New day");
    }
    private void DayManager_OnDayEnd(object sender, EventArgs e)
    {
        dayTransitionUI.SetActive(true);
    }
}
