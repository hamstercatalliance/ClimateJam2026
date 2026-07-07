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
    [SerializeField] private float secondsInADay = 300f;
    private float minutesInADay = 5f;
    [SerializeField] private float timeElapsed = 0f;
    [SerializeField] private Volume day;
    [SerializeField] private Volume night;
    [SerializeField] private Volume transition;
    private int dayCount = 0;

    public enum State
    {
        Sunrising,
        Daytime,
        Sunsetting,
        Moonrising,
        Nighttime,
    }
    private State state;
    public static DayManager Instance { get; private set; } //DayManager singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        state = State.Sunrising;
        minutesInADay = secondsInADay / 60f;
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        PostProcessVolumeTransition(GetProgressNormalized());
        // OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        // {
        //     progressNormalized = timeElapsed / secondsInADay
        // });
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
    private void PostProcessVolumeTransition(float progress)
    {
        // sunrise->day : 0.65 minute
        // day : 2.75 minutes
        // day->sunset : 0.5 minute
        // sunset->night : 0.4 minute
        // night : 0.7 minute

        float sunriseDayTransitionWeight = 0.65f / minutesInADay;
        float dayWeight = 2.75f / minutesInADay;
        float daySunsetTransitionWeight = 0.5f / minutesInADay;
        float sunsetNightTransitionWeight = 0.4f / minutesInADay;
        float nightWeight = 0.7f / minutesInADay;
        
        switch (state)
        {
            case State.Sunrising:
                day.weight = Mathf.Clamp01(progress / sunriseDayTransitionWeight);
                transition.weight = Mathf.Clamp01(1f - (progress / sunriseDayTransitionWeight));
                night.weight = 0f;
                if (progress >= sunriseDayTransitionWeight)
                {
                    Debug.Log("Switching to daytime");
                    state = State.Daytime;
                }
                break;
            case State.Daytime:
                day.weight = 1f;
                transition.weight = 0f;
                night.weight = 0f;
                if (progress >= sunriseDayTransitionWeight + dayWeight)
                {
                    Debug.Log("Switching to sunset");
                    state = State.Sunsetting;
                }
                break;
            case State.Sunsetting:
                day.weight = Mathf.Clamp01(1f - ((progress - (sunriseDayTransitionWeight + dayWeight)) / daySunsetTransitionWeight));
                transition.weight = Mathf.Clamp01((progress - (sunriseDayTransitionWeight + dayWeight)) / daySunsetTransitionWeight);
                night.weight = 0f;
                if (progress >= sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight)
                {
                    Debug.Log("Switching to moonrising");
                    state = State.Moonrising;

                    OnMoonrise?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Moonrising:
                day.weight = 0f;
                transition.weight = Mathf.Clamp01(1f - ((progress - (sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight)) / sunsetNightTransitionWeight));
                night.weight = Mathf.Clamp01((progress - (sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight)) / sunsetNightTransitionWeight);
                if (progress >= sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight + sunsetNightTransitionWeight)
                {
                    Debug.Log("Switching to nighttime");
                    state = State.Nighttime;
                }
                break;
            case State.Nighttime:
                day.weight = 0f;
                transition.weight = 0f;
                night.weight = 1f;
                if (progress >= sunriseDayTransitionWeight + dayWeight + daySunsetTransitionWeight + sunsetNightTransitionWeight + nightWeight)
                {
                    Debug.Log("New day");
                    state = State.Sunrising;
                }
                break;
        }
    }
}
