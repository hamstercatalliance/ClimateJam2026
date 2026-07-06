using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayManagerUI : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        startPos = progressBarStartPoint.transform.position;
        endPos = progressBarEndPoint.transform.position;

        progressTracker.transform.position = startPos;
        
        sun.SetActive(true);
        moon.SetActive(true);
        SetGameObjectImageAlpha(sun, 1f);
        SetGameObjectImageAlpha(moon, 0f);

        DayManager.Instance.OnDayChanged += DayManager_OnDayChanged;
        DayManager.Instance.OnMoonrise += DayManager_OnMoonrise;
    }
    private void DayManager_OnMoonrise(object sender, System.EventArgs e)
    {
        StartCoroutine(TransitionFade(20f));
    }
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
        //Debug.Log("Sun Position: " + sun.transform.localPosition + " Progress Normalized: " + progressNormalized);
    }
    private IEnumerator TransitionFade(float duration)
    {
        Debug.Log("Transitioning from sun to moon");
        Image sunImg = sun.GetComponent<Image>();
        Image moonImg = moon.GetComponent<Image>();
        float t = 0f;
        Debug.Log(t < duration);
        while (t < duration)
        {
            float delta = Time.deltaTime / duration;
            
            //sun out
            Color sc = sunImg.color;
            sc.a = Mathf.Clamp01(sc.a - delta);
            sunImg.color = sc;

            //moon in
            Color mc = moonImg.color;
            mc.a = Mathf.Clamp01(mc.a + delta);
            moonImg.color = mc;

            Debug.Log("Sun Alpha: " + sc.a + " Moon Alpha: " + mc.a);
            t += Time.deltaTime;
            yield return null;
        }

        SetGameObjectImageAlpha(sun, 0f);
        SetGameObjectImageAlpha(moon, 1f);
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
}
