using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEngine.SceneManagement;
public class DayManager : MonoBehaviour, IHasPersistentData//, IHasProgress
{
    [SerializeField] private DayCountdown dayCountdown;
    private const string GOOD_END_SCENE = "GoodEnd";
    private const string BAD_END_SCENE = "BadEnd";
    private const float EPSILON = 1e-5f;

    public EventHandler<OnDayChangedEventArgs> OnDayChanged;
    public class OnDayChangedEventArgs : EventArgs
    {
        public int day;
    }
    public event EventHandler OnDayManagerDataLoaded;
    public bool HasFiredDataLoaded { get; private set; }
    public event EventHandler OnDayEnd;

    [Header("Change this to modify day length")]
    [SerializeField] private float secondsInADay = 300f;
    //private float minutesInADay;
    [Header("Only modify for playtesting purposes")]
    [SerializeField] private float timeElapsed = 0f;
    [Header("Do not touch these")]
    [SerializeField] private Volume day;
    [SerializeField] private Volume night;
    [SerializeField] private Volume transition;
    public int dayCount { get; private set; } = 0;
    public bool DataSuccessfullyWritten { get; private set; }
    // sunrise->day : 0.65 minute
    // day : 2.75 minutes
    // day->sunset : 0.5 minute
    // sunset->night : 0.4 minute
    // night : 0.7 minute
    // [SerializeField] private float sunriseDayTransitionMinutes = 0.65f;
    // [SerializeField] private float dayMinutes = 2.75f;
    // [SerializeField] private float daySunsetTransitionMinutes = 0.5f;
    // [SerializeField] private float sunsetNightTransitionMinutes = 0.4f;
    // [SerializeField] private float nightMinutes = 0.7f;
    [Header("Day phase lengths in percent (must sum to 1)")]
    [SerializeField, Range(0f, 1f)] private float sunriseDayTransitionPercent = 0.13f;
    [SerializeField, Range(0f, 1f)] private float dayPercent = 0.55f;
    [SerializeField, Range(0f, 1f)] private float daySunsetTransitionPercent = 0.10f;
    [SerializeField, Range(0f, 1f)] private float sunsetNightTransitionPercent = 0.08f;
    [SerializeField, Range(0f, 1f)] private float nightPercent = 0.14f;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        float total = sunriseDayTransitionPercent + dayPercent + daySunsetTransitionPercent + sunsetNightTransitionPercent + nightPercent;
        if (Mathf.Abs(total - 1f) > 0.001f)
        {
            Debug.LogWarning($"Day phase percentages sum to {total}, should sum to 1.0");
        }
    }
    #endif

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
    }
    private void Start()
    {
        DynamicGI.UpdateEnvironment();//This thing fixes the weird lighting issue upon loadscene

        //minutesInADay = secondsInADay / 60f;
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

            if (timeElapsed < EPSILON)
            {
                //Show days remaining at the start of each day
            }
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
        // float sunriseDayTransitionWeight = sunriseDayTransitionMinutes / minutesInADay;
        // float dayWeight = dayMinutes / minutesInADay;
        // float daySunsetTransitionWeight = daySunsetTransitionMinutes / minutesInADay;
        // float sunsetNightTransitionWeight = sunsetNightTransitionMinutes / minutesInADay;

        // float moonriseStart = sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight;
        // float progress = GetProgressNormalized();

        // return Mathf.Clamp01((progress - moonriseStart) / sunsetNightTransitionWeight);

        float sunsetEnd = sunriseDayTransitionPercent + dayPercent + daySunsetTransitionPercent;
        float progress = GetProgressNormalized();
        return Mathf.Clamp01((progress - sunsetEnd) / sunsetNightTransitionPercent);
    }
    private void PostProcessVolumeTransition(float progressNormalized)
    {
        // float sunriseDayTransitionWeight = sunriseDayTransitionMinutes / minutesInADay;
        // float dayWeight = dayMinutes / minutesInADay;
        // float daySunsetTransitionWeight = daySunsetTransitionMinutes / minutesInADay;
        // float sunsetNightTransitionWeight = sunsetNightTransitionMinutes / minutesInADay;
        // float nightWeight = nightMinutes / minutesInADay;
        float sunriseEnd = sunriseDayTransitionPercent;
        float dayEnd = sunriseEnd + dayPercent;
        float sunsetEnd = dayEnd + daySunsetTransitionPercent;
        float moonriseEnd = sunsetEnd + sunsetNightTransitionPercent;

        switch (state)
        {
            case State.Sunrising:
                day.weight = Mathf.Clamp01(progressNormalized / sunriseEnd);
                transition.weight = Mathf.Clamp01(1f - (progressNormalized / sunriseEnd));
                night.weight = 0f;
                if (progressNormalized >= sunriseEnd)
                {
                    Debug.Log("Switching to daytime");
                    state = State.Daytime;
                }
                break;
            case State.Daytime:
                day.weight = 1f;
                transition.weight = 0f;
                night.weight = 0f;
                if (progressNormalized >= dayEnd)
                {
                    Debug.Log("Switching to sunset");
                    state = State.Sunsetting;
                }
                break;
            case State.Sunsetting:
                day.weight = Mathf.Clamp01(1f - ((progressNormalized - dayEnd) / daySunsetTransitionPercent));
                transition.weight = Mathf.Clamp01((progressNormalized - dayEnd) / daySunsetTransitionPercent);
                night.weight = 0f;
                if (progressNormalized >= sunsetEnd)
                {
                    Debug.Log("Switching to moonrising");
                    state = State.Moonrising;

                    //OnMoonrise?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Moonrising:
                day.weight = 0f;
                transition.weight = Mathf.Clamp01(1f - ((progressNormalized - sunsetEnd) / sunsetNightTransitionPercent));
                night.weight = Mathf.Clamp01((progressNormalized - sunsetEnd) / sunsetNightTransitionPercent);
                if (progressNormalized >= moonriseEnd)
                {
                    Debug.Log("Switching to nighttime");
                    state = State.Nighttime;
                }
                break;
            case State.Nighttime:
                day.weight = 0f;
                transition.weight = 0f;
                night.weight = 1f;
                if (progressNormalized >= 1f)
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

        if (dayCount > dayCountdown.GetCountdownDays())
        {
            //TRIGGER ENDINGS
            // if (SympathyPointsManager.Instance.HasReachedGoodEndingThreshold())
            // {
            //     Debug.Log("Good ending triggered.");
            //     SceneManager.LoadScene(GOOD_END_SCENE);
            // }
            // else
            // {
            //     Debug.Log("Bad ending triggered.");
            //     SceneManager.LoadScene(BAD_END_SCENE);
            // }
            // return;
        }
        
        timeElapsed = 0f;
        GameData.Instance.HasCompletedFirstDay = true;
        DayManagerUI.Instance.ResetTransitionProgress();
        //Debug.Log("Day complete. Showing transition screen.");
        OnDayChanged?.Invoke(this, new OnDayChangedEventArgs 
        { 
            day = dayCount 
        });
        OnDayEnd?.Invoke(this, EventArgs.Empty);
    }
    private State GetStateFromProgress(float timeElapsed)
    {
        float progress = timeElapsed / secondsInADay; // was: timeElapsed / 60f (minutes)
        float sunriseEnd = sunriseDayTransitionPercent;
        float dayEnd = sunriseEnd + dayPercent;
        float sunsetEnd = dayEnd + daySunsetTransitionPercent;
        float moonriseEnd = sunsetEnd + sunsetNightTransitionPercent;

        if (progress < sunriseEnd)
        {
            return State.Sunrising;
        }
        else if (progress < dayEnd)
        {
            return State.Daytime;
        }
        else if (progress < sunsetEnd)
        {
            return State.Sunsetting;
        }
        else if (progress < moonriseEnd)
        {
            return State.Moonrising;
        }
        else
        {
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
