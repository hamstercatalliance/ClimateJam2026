using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; } //DayManager singleton
    private void Awake()
    {
        Instance = this;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private float secondsInADay = 300f;
    private float timeElapsed = 0f;
    private int dayCount = 0;
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= secondsInADay)
        {
            timeElapsed = 0f;
            dayCount++;
        }
    }
}
