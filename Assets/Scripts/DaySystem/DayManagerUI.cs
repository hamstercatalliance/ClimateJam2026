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
    //private float transitionProgress;
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
        //DayManager.Instance.OnMoonrise += DayManager_OnMoonrise;
        //SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        DayManager.Instance.OnDayManagerDataLoaded += DayManager_OnDayManagerDataLoaded;
        if (DayManager.Instance.HasFiredDataLoaded)
        {
            // we subscribed too late, event already fired — call it manually
            LoadDayData();
        }
        startPos = progressBarStartPoint.transform.position;
        endPos = progressBarEndPoint.transform.position;

        // progressTracker.transform.position = startPos;
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
            //DayManager.State state = DayManager.Instance.GetState();
            dayCountText.text = "Day " + GameData.Instance.DayManagerDayCount;
            Debug.Log("Game data found.");
            // // Load UI elements based on saved game data
            // if (GameData.Instance.DayManagerUITransitionProgress != 0f)
            // {
            //     Debug.Log("In the middle of a transition (moonrising).");
            //     transitionProgress = GameData.Instance.DayManagerUITransitionProgress;
            //     ContinueTransition(transitionProgress);
            // }
            // else
            // {
                // Debug.Log("No transition in progress.");
                // if (state == DayManager.State.Moonrising || state == DayManager.State.Nighttime)
                // {
                    
                //     SetGameObjectImageAlpha(sun, 0f);
                //     SetGameObjectImageAlpha(moon, 1f);
                // }
                // else if (state == DayManager.State.Sunrising || state == DayManager.State.Daytime)
                // {
                //     SetGameObjectImageAlpha(sun, 1f);
                //     SetGameObjectImageAlpha(moon, 0f);
                // }
                // Debug.Log(state);
            // }
        }
        else
        {
            Debug.Log("No game data found. Initializing default values.");
            // // Initialize UI elements
            // SetGameObjectImageAlpha(sun, 1f);
            // SetGameObjectImageAlpha(moon, 0f);
            // transitionProgress = 0;
        }
    }
    // private void OnSceneTransitionHandler(object sender, EventArgs e)
    // {
    //     WriteToGameData();
    // }
    // private void ContinueTransition(float? transitionProgress)
    // {
    //     float transitionProgressNormalized = transitionProgress.Value / spriteTransitionDuration;
    //     SetGameObjectImageAlpha(sun, 1f - transitionProgressNormalized);
    //     SetGameObjectImageAlpha(moon, transitionProgressNormalized);
    //     StartCoroutine(TransitionFade(spriteTransitionDuration-transitionProgress.Value));
    // }
    // private void DayManager_OnMoonrise(object sender, System.EventArgs e)
    // {
    //     transitionProgress = 0f;
    //     StartCoroutine(TransitionFade(spriteTransitionDuration-transitionProgress));
    // }
    private void DayManager_OnDayChanged(object sender, DayManager.OnDayChangedEventArgs e)
    {
        dayCountText.text = "Day " + e.day;
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
    // private IEnumerator TransitionFade(float duration)
    // {
    //     Debug.Log("Transitioning from sun to moon");
    //     Image sunImg = sun.GetComponent<Image>();
    //     Image moonImg = moon.GetComponent<Image>();
    //     float t = 0f;
    //     Debug.Log(t < duration);
    //     while (t < duration)
    //     {
    //         float delta = Time.deltaTime / duration;
            
    //         //sun out
    //         Color sc = sunImg.color;
    //         sc.a = Mathf.Clamp01(sc.a - delta);
    //         sunImg.color = sc;

    //         //moon in
    //         Color mc = moonImg.color;
    //         mc.a = Mathf.Clamp01(mc.a + delta);
    //         moonImg.color = mc;

    //         Debug.Log("Sun Alpha: " + sc.a + " Moon Alpha: " + mc.a);
    //         t += Time.deltaTime;
    //         transitionProgress += Time.deltaTime;

    //         yield return null;
    //     }

    //     SetGameObjectImageAlpha(sun, 0f);
    //     SetGameObjectImageAlpha(moon, 1f);
    //     //GameData.Instance.TransitionProgress = null;
    //     transitionProgress = 0;
    // }
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
    // public void WriteToGameData()
    // {
        //GameData.Instance.DayManagerUITransitionProgress = transitionProgress;
        //DataSuccessfullyWritten = true;
    // }
}
