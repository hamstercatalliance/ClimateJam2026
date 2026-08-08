using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class DayEndCountdown : MonoBehaviour, IHasPersistentData
{
    [SerializeField] private TextMeshProUGUI countdownText;
    public bool DataSuccessfullyWritten { get; private set; }
    private float timeElapsed = 0f;
    private bool isCountingDown = false;

    public static event EventHandler OnCountdownStarted;

    private void Start()
    {
        LoadGameData();
        DayManager.Instance.OnFourSecondsLeftInDay += OnFourSecondsLeftInDayHandler;
        SceneLoader.OnSceneTransition += SceneLoader_OnSceneTransition;
    }
    private void SceneLoader_OnSceneTransition(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    private void OnFourSecondsLeftInDayHandler(object sender, EventArgs e)
    {
        countdownText.text = "It's almost bedtime!";
        OnCountdownStarted?.Invoke(this, EventArgs.Empty);
        StartCoroutine(Timer());
    }
    private void OnDestroy()
    {
        DayManager.Instance.OnFourSecondsLeftInDay -= OnFourSecondsLeftInDayHandler;
        SceneLoader.OnSceneTransition -= SceneLoader_OnSceneTransition;
    }
    private IEnumerator Timer(float progress = 0f)
    {
        isCountingDown = true;
        while (progress < 4f)
        {
            progress += Time.deltaTime;
            timeElapsed = progress;
            if (progress >= 0f && progress < 1f)
            {
                countdownText.text = "It's time for bed!";
            }
            else if (progress >= 1f && progress < 2f)
            {
                countdownText.text = "3";
            }
            else if (progress >= 2f && progress < 3f)
            {
                countdownText.text = "2";
            }
            else if (progress >= 3f && progress < 4f)
            {
                countdownText.text = "1";
            }
            else
            {
                countdownText.text = "0";
            }
            yield return null;
        }
        isCountingDown = false;
        timeElapsed = 0f;
        WriteToGameData();
    }

    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            timeElapsed = GameData.Instance.EndOfDayCountdownTimeElapsed;
            isCountingDown = GameData.Instance.IsCountingDown;
            Debug.Log("Loaded countdown time elapsed: " + timeElapsed);
            Debug.Log("Loaded countdown is counting down: " + isCountingDown);
            if (isCountingDown)
            {
                StartCoroutine(Timer(timeElapsed));
            }
            else
            {
                countdownText.text = "";
            }
        }
        else
        {
            Debug.Log("no game data found, hiding countdown text");
            timeElapsed = 0f;
            isCountingDown = false;
            countdownText.text = "";
        }
    }

    public void WriteToGameData()
    {
        if (GameData.Instance != null)
        {
            GameData.Instance.EndOfDayCountdownTimeElapsed = timeElapsed;
            GameData.Instance.IsCountingDown = isCountingDown;
        }
    }
}
