using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class DayManager : MonoBehaviour, IHasPersistentData//, IHasProgress
{
    public EventHandler<OnDayChangedEventArgs> OnDayChanged;
    public class OnDayChangedEventArgs : EventArgs
    {
        public int day;
    }
    public event EventHandler OnDayManagerDataLoaded;
    public bool HasFiredDataLoaded { get; private set; }
    public event EventHandler OnDayEnd;
    [SerializeField] private float secondsInADay = 300f;
    private float minutesInADay;
    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private Volume day;
    [SerializeField] private Volume night;
    [SerializeField] private Volume transition;
    private int dayCount = 0;
    public bool DataSuccessfullyWritten { get; private set; }
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
        DayEnded
    }
    private State state;
    public State GetState()
    {
        return state;
    }
    public void SetState(State newState)
    {
        state = newState;
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
        DynamicGI.UpdateEnvironment();//This thing fixes the weird lighting issue upon loadscene

        minutesInADay = secondsInADay / 60f;
        LoadGameData();
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    private void OnSceneTransitionHandler(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            // Debug.Log("Game data found.");
            timeElapsed = GameData.Instance.DayManagerTimeElapsed;
            dayCount = GameData.Instance.DayManagerDayCount;
            state = GetStateFromProgress(timeElapsed);
            Debug.Log("Time elapsed: " + timeElapsed + "/" + secondsInADay);
        }
        else
        {
            // Debug.Log("No game data found. Initializing default values.");
            timeElapsed = 0f;
            dayCount = 0;
            state = State.Sunrising;
        }
        OnDayManagerDataLoaded?.Invoke(this, EventArgs.Empty);
        HasFiredDataLoaded = true;
    }
    private void Update()
    {
        if (state == State.DayEnded)
        {
            return; // frozen-- no more time ticking & no more volume changes until scene reload
        }
        timeElapsed += Time.deltaTime;
        PostProcessVolumeTransition(GetProgressNormalized());
    }
    public float GetProgressNormalized()
    {
        return timeElapsed / secondsInADay;
    }
    public float GetMoonriseProgressNormalized()
    {
        float sunriseDayTransitionWeight = sunriseDayTransitionMinutes / minutesInADay;
        float dayWeight = dayMinutes / minutesInADay;
        float daySunsetTransitionWeight = daySunsetTransitionMinutes / minutesInADay;
        float sunsetNightTransitionWeight = sunsetNightTransitionMinutes / minutesInADay;

        float moonriseStart = sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight;
        float progress = GetProgressNormalized();

        return Mathf.Clamp01((progress - moonriseStart) / sunsetNightTransitionWeight);
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

                    //OnMoonrise?.Invoke(this, EventArgs.Empty);
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
                    EndDay();
                }
                break;
        }
    }
    public void EndDay()
    {
        if (state == State.DayEnded)
        {
            return;
        }

        state = State.DayEnded;
        dayCount++;
        timeElapsed = 0f;
        DayManagerUI.Instance.ResetTransitionProgress();
        Debug.Log("Day complete. Showing transition screen.");
        OnDayChanged?.Invoke(this, new OnDayChangedEventArgs 
        { 
            day = dayCount 
        });
        OnDayEnd?.Invoke(this, EventArgs.Empty);
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
            Debug.Log(timeElapsed);
            return State.Nighttime;
        }
    }
    private void WriteTimeAndDayToGameData(float time, int day)
    {
        GameData.Instance.DayManagerTimeElapsed = time;
        GameData.Instance.DayManagerDayCount = day;
        DataSuccessfullyWritten = true;
        Debug.Log("Wrote day data. Time: " + time + " Day: " + day);
    }
    public void WriteToGameData() //this is called when the scene is transitioning
    {
        WriteTimeAndDayToGameData(timeElapsed, dayCount);
    }
}
