using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class DayManager : MonoBehaviour//, IHasProgress
{
    //public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnMoonrise;
    public EventHandler<OnDayChangedEventArgs> OnDayChanged;
    public class OnDayChangedEventArgs : EventArgs
    {
        public int day;
    }
    public event EventHandler OnDayManagerDataLoaded;
    [SerializeField] private float secondsInADay = 300f;
    private float minutesInADay;
    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private Volume day;
    [SerializeField] private Volume night;
    [SerializeField] private Volume transition;
    private int dayCount = 0;

    // sunrise->day : 0.65 minute
    // day : 2.75 minutes
    // day->sunset : 0.5 minute
    // sunset->night : 0.4 minute
    // night : 0.7 minute
    [SerializeField] private float sunriseDayTransitionMinutes = 0.65f;
    [SerializeField] private float dayMinutes = 2.75f;
    [SerializeField] private float daySunsetTransitionMinutes = 0.5f;
    [SerializeField] private float sunsetNightTransitionMinutes = 0.4f;
    [SerializeField] private float nightMinutes = 0.7f;
    public enum State
    {
        Sunrising,
        Daytime,
        Sunsetting,
        Moonrising,
        Nighttime,
    }
    private State state;
    public State GetState()
    {
        return state;
    }
    public static DayManager Instance { get; private set; } //DayManager singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        minutesInADay = secondsInADay / 60f;
        LoadDayData();
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
    }
    private void OnSceneTransitionHandler(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    private void LoadDayData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            // Debug.Log("Game data found.");
            timeElapsed = GameData.Instance.DayManagerTimeElapsed;
            dayCount = GameData.Instance.DayManagerDayCount;
            state = GetStateFromProgress(timeElapsed);
            Debug.Log("Time elapsed: " + timeElapsed + "/" + secondsInADay);
            OnDayChanged?.Invoke(this, new OnDayChangedEventArgs
            {
                day = dayCount
            });
        }
        else
        {
            // Debug.Log("No game data found. Initializing default values.");
            timeElapsed = 0f;
            dayCount = 0;
            state = State.Sunrising;

            if (GameData.Instance != null)
            {
                WriteToGameData();
                GameData.Instance.HasLoadedRunData = true;
            }
        }

        OnDayManagerDataLoaded?.Invoke(this, EventArgs.Empty);
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        PostProcessVolumeTransition(GetProgressNormalized());
        if (timeElapsed >= secondsInADay)
        {
            timeElapsed = 0f;
            dayCount++;
            OnDayChanged?.Invoke(this, new OnDayChangedEventArgs
            {
                day = dayCount
            });
        }
    }
    public float GetProgressNormalized()
    {
        return timeElapsed / secondsInADay;
    }
    private void PostProcessVolumeTransition(float progressNormalized)
    {
        float sunriseDayTransitionWeight = sunriseDayTransitionMinutes / minutesInADay;
        float dayWeight = dayMinutes / minutesInADay;
        float daySunsetTransitionWeight = daySunsetTransitionMinutes / minutesInADay;
        float sunsetNightTransitionWeight = sunsetNightTransitionMinutes / minutesInADay;
        float nightWeight = nightMinutes / minutesInADay;
        
        switch (state)
        {
            case State.Sunrising:
                day.weight = Mathf.Clamp01(progressNormalized / sunriseDayTransitionWeight);
                transition.weight = Mathf.Clamp01(1f - (progressNormalized / sunriseDayTransitionWeight));
                night.weight = 0f;
                if (progressNormalized >= sunriseDayTransitionWeight)
                {
                    Debug.Log("Switching to daytime");
                    state = State.Daytime;
                }
                break;
            case State.Daytime:
                day.weight = 1f;
                transition.weight = 0f;
                night.weight = 0f;
                if (progressNormalized >= sunriseDayTransitionWeight + dayWeight)
                {
                    Debug.Log("Switching to sunset");
                    state = State.Sunsetting;
                }
                break;
            case State.Sunsetting:
                day.weight = Mathf.Clamp01(1f - ((progressNormalized - (sunriseDayTransitionWeight + dayWeight)) / daySunsetTransitionWeight));
                transition.weight = Mathf.Clamp01((progressNormalized - (sunriseDayTransitionWeight + dayWeight)) / daySunsetTransitionWeight);
                night.weight = 0f;
                if (progressNormalized >= sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight)
                {
                    Debug.Log("Switching to moonrising");
                    state = State.Moonrising;

                    OnMoonrise?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Moonrising:
                day.weight = 0f;
                transition.weight = Mathf.Clamp01(1f - ((progressNormalized - (sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight)) / sunsetNightTransitionWeight));
                night.weight = Mathf.Clamp01((progressNormalized - (sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight)) / sunsetNightTransitionWeight);
                if (progressNormalized >= sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight + sunsetNightTransitionWeight)
                {
                    Debug.Log("Switching to nighttime");
                    state = State.Nighttime;
                }
                break;
            case State.Nighttime:
                day.weight = 0f;
                transition.weight = 0f;
                night.weight = 1f;
                if (progressNormalized >= sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight + sunsetNightTransitionWeight + nightWeight)
                {
                    Debug.Log("New day");
                    state = State.Sunrising;
                }
                break;
        }
    }
    private State GetStateFromProgress(float timeElapsed)
    {
        float progress = timeElapsed / 60f; // Convert to minutes
        Debug.Log("Minutes: " + progress);
        if (progress < sunriseDayTransitionMinutes)
        {
            Debug.Log("Setting state to sunrising");
            return State.Sunrising;
        }
        else if (progress < sunriseDayTransitionMinutes + dayMinutes)
        {
            return State.Daytime;
        }
        else if (progress < sunriseDayTransitionMinutes + dayMinutes + daySunsetTransitionMinutes)
        {
            return State.Sunsetting;
        }
        else if (progress < sunriseDayTransitionMinutes + dayMinutes + daySunsetTransitionMinutes + sunsetNightTransitionMinutes)
        {
            return State.Moonrising;
        }
        else
        {
            return State.Nighttime;
        }
    }
    private void WriteToGameData()
    {
        GameData.Instance.DayManagerTimeElapsed = timeElapsed;
        GameData.Instance.DayManagerDayCount = dayCount;
    }
}
