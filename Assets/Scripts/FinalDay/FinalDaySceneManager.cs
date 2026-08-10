using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FinalDaySceneManager : SpatialSFX //MADE FOR DOWNTOWN
{
    [SerializeField] private AudioClip dataCenterSounds;
    [SerializeField] private AudioClip protestSounds;

    [SerializeField] private GameObject newBorders;
    [SerializeField] private GameObject constructionSite;
    [SerializeField] private GameObject exitPortal;

    [SerializeField] private GameObject protest;

    [SerializeField] private GameObject endGameTrigger;
    [SerializeField] private GameObject blackScreen;

    public bool IsFading { get; private set; }
    private float fadeDuration = 10f;

    private const string GOOD_ENDING_SCENE = "GoodEnd";
    private const string BAD_ENDING_SCENE = "BadEnd";

    public static FinalDaySceneManager Instance { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        blackScreen.SetActive(false);
        if (FinalDayEndingManager.Instance.IsFinalDay())
        {
            newBorders.SetActive(true);
            constructionSite.SetActive(false);
            exitPortal.SetActive(false); //can't go back home
            endGameTrigger.SetActive(true); //when stepped in, will fade out to ending cutscene
            if (SympathyPointsManager.Instance.HasReachedGoodEndingThreshold())
            {
                protest.SetActive(true);
            }
            else
            {
                protest.SetActive(false);
            }
            return;
        }
        endGameTrigger.SetActive(false);
        protest.SetActive(false);
    }
    public void BadEnd()
    {
        audioSource.clip = dataCenterSounds;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void GoodEnd()
    {
        audioSource.clip = protestSounds;
        audioSource.loop = true;
        audioSource.Play();

        //show the protest signs and animations
    }

    public void ShowBlackScreen()
    {
        Image image = blackScreen.GetComponent<Image>();
        Color imageColor = image.color;
        imageColor.a = 0f;
        blackScreen.SetActive(true);
        IsFading = true;
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator FadeInBlackScreen()
    {
        Image image = blackScreen.GetComponent<Image>();
        Color imageColor = image.color;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            imageColor.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            image.color = imageColor;
            yield return null;
        }
        if (SympathyPointsManager.Instance.HasReachedGoodEndingThreshold())
        {
            // Load the good ending scene
            SceneManager.LoadScene(GOOD_ENDING_SCENE);
        }
        else
        {
            // Load the bad ending scene
            SceneManager.LoadScene(BAD_ENDING_SCENE);
        }
    }

}
