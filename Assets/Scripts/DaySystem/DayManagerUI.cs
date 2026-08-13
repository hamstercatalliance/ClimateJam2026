using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
public class DayManagerUI : MonoBehaviour//, IHasPersistentData
{
    public static DayManagerUI Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    //[SerializeField] private float spriteTransitionDuration = 20f;
    [SerializeField] private GameObject progressBarStartPoint;
    [SerializeField] private GameObject progressBarEndPoint;
    //[SerializeField] private DayManager dayManager;
    [SerializeField] private GameObject progressTracker;
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject moon;
    [SerializeField] private TextMeshProUGUI dayCountText;
    private Vector3 startPos;
    private Vector3 endPos;
    private float progressNormalized;
    public void ResetTransitionProgress()
    {
        //transitionProgress = 0f;
        SetGameObjectImageAlpha(sun, 1f);
        SetGameObjectImageAlpha(moon, 0f);
    }
    public bool DataSuccessfullyWritten { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        DayManager.Instance.OnDayChanged += DayManager_OnDayChanged;
        DayManager.Instance.OnDayManagerDataLoaded += DayManager_OnDayManagerDataLoaded;
        if (DayManager.Instance.HasFiredDataLoaded)
        {
            // we subscribed too late, event already fired — call it manually
            LoadDayData();
        }
        startPos = progressBarStartPoint.transform.position;
        endPos = progressBarEndPoint.transform.position;

        progressTracker.transform.position = DayManager.Instance.GetProgressNormalized() * (endPos - startPos) + startPos;

        sun.SetActive(true);
        moon.SetActive(true);
    }
    private void DayManager_OnDayManagerDataLoaded(object sender, EventArgs e)
    {
        //ALWAYS LOAD DAY MANAGER DATA BEFORE DAY MANAGER UI DATA
        LoadDayData();
    }
    public void LoadDayData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            int dayDisplay = GameData.Instance.DayManagerDayCount + 1;
            dayCountText.text = "Day\n" + dayDisplay;
            Debug.Log("Game data found.");
        }
        else
        {
            dayCountText.text = "Day\n1";
            Debug.Log("No game data found. Initializing default values.");
        }
    }
    private void DayManager_OnDayChanged(object sender, DayManager.OnDayChangedEventArgs e)
    {
        int dayDisplay = e.day + 1;
        dayCountText.text = "Day\n" + dayDisplay;
        SetGameObjectImageAlpha(sun, 1f);
        SetGameObjectImageAlpha(moon, 0f);
    }
    // Update is called once per frame
    void Update()
    {
        progressNormalized = DayManager.Instance.GetProgressNormalized();
        if (progressNormalized >= 1f)
        {
            progressTracker.transform.position = startPos;
        }
        progressTracker.transform.position = Vector3.Lerp(startPos, endPos, progressNormalized);
        UpdateSunMoonVisual();
        //Debug.Log("Sun Position: " + sun.transform.localPosition + " Progress Normalized: " + progressNormalized);
    }
    private void UpdateSunMoonVisual()
    {
        DayManager.State state = DayManager.Instance.GetState();

        if (state == DayManager.State.Moonrising)
        {
            float t = DayManager.Instance.GetMoonriseProgressNormalized();
            SetGameObjectImageAlpha(sun, 1f - t);
            SetGameObjectImageAlpha(moon, t);
        }
        else if (state == DayManager.State.Nighttime || state == DayManager.State.DayEnded)
        {
            SetGameObjectImageAlpha(sun, 0f);
            SetGameObjectImageAlpha(moon, 1f);
        }
        else //sunrising, daytime, sunsetting
        {
            SetGameObjectImageAlpha(sun, 1f);
            SetGameObjectImageAlpha(moon, 0f);
        }
    }
    private void SetGameObjectImageAlpha(GameObject obj, float alpha)
    {
        Image img = obj.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = Mathf.Clamp01(alpha);
            img.color = c;
        }
    }
    public void HideAll()
    {
        //disable this gameobject
        gameObject.SetActive(false);
    }
}
