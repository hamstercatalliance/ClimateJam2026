using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
public class DayEndCutscenePlayer : MonoBehaviour
{
    public static event EventHandler OnCutsceneStart;

    //EVENTUALLY REFACTOR TO SUPPORT 2 TYPES OF CUTSCENES: 
    // starting at door (early end) & walking up to door (end of day)
    [SerializeField] private VideoPlayer endNormalCutscene; 
    [SerializeField] private GameObject videoPanel; //displays the cutscene
    [SerializeField] private GameObject screenFade;
    private AnimationClip fadeClip;
    private string videoFileName = "EndDay.mp4";
    private const float INITIAL_FADE_DURATION = 1.7f;
    private Animator screenFadeAnimator;
    public static DayEndCutscenePlayer Instance { get; private set; }
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
#if UNITY_WEBGL && !UNITY_EDITOR
                string webglPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
                endNormalCutscene.source = VideoSource.Url;
                endNormalCutscene.url = webglPath;
#else
        string desktopPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        endNormalCutscene.source = VideoSource.Url;
        endNormalCutscene.url = desktopPath;
#endif
        endNormalCutscene.Prepare();
        videoPanel.SetActive(false);
        screenFadeAnimator = screenFade.GetComponent<Animator>();
        screenFadeAnimator.Play("BasicUnfade");
    }
    public IEnumerator PlayEarlyEndCutscene()
    {
        //IMPLEMENT HERE
        yield return null;
    }
    public IEnumerator PlayEndOfDayCutscene()
    {
        screenFade.SetActive(true);
        screenFadeAnimator.Play("BasicFade");
        yield return null;
        yield return new WaitUntil(() => screenFadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        screenFadeAnimator.Play("SceneTransitionFade");
        videoPanel.SetActive(true);
        endNormalCutscene.time = 1.5f;
        endNormalCutscene.Play();
        
        yield return new WaitUntil(() => endNormalCutscene.isPlaying);
        StartCoroutine(PlayJingleAfterDelay(2.5f));
        yield return new WaitWhile(() => endNormalCutscene.isPlaying);

       screenFadeAnimator.Play("BasicFade");
        yield return null;
        yield return new WaitUntil(() => screenFadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    private IEnumerator PlayJingleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnCutsceneStart?.Invoke(this, EventArgs.Empty);
    }
}
