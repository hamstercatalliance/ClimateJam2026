using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
public class DayEndCutscenePlayer : MonoBehaviour
{
    public static event EventHandler OnCutsceneStart;

    //EVENTUALLY REFACTOR TO SUPPORT 2 TYPES OF CUTSCENES: 
    // starting at door (early end) & walking up to door (end of day)
    [SerializeField] private VideoPlayer endNormalCutscene; 
    [SerializeField] private GameObject videoPanel; //displays the cutscene
    [SerializeField] private GameObject screenFade;
    private AnimationClip fadeClip;
    private const float INITIAL_FADE_DURATION = 1.7f;
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
        videoPanel.SetActive(false);
        fadeClip = screenFade.GetComponent<Animator>().runtimeAnimatorController.animationClips[0];
    }
    public IEnumerator PlayEarlyEndCutscene()
    {
        //IMPLEMENT HERE
        yield return null;
    }
    public IEnumerator PlayEndOfDayCutscene()
    {
        screenFade.SetActive(true);
        yield return new WaitForSeconds(INITIAL_FADE_DURATION);
        
        OnCutsceneStart?.Invoke(this, EventArgs.Empty);
        videoPanel.SetActive(true);
        endNormalCutscene.Play();

        yield return new WaitForSeconds(fadeClip.length - INITIAL_FADE_DURATION);
    }
}
