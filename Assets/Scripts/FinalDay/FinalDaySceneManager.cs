using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class FinalDaySceneManager : SpatialSFX //MADE FOR DOWNTOWN
{
    [SerializeField] private AudioClip dataCenterSounds;
    [SerializeField] private AudioClip protestSounds;

    [SerializeField] private GameObject oldBorders;
    [SerializeField] private GameObject newBorders;
    [SerializeField] private GameObject constructionSite;
    [SerializeField] private GameObject exitPortal;

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
        if (FinalDayEndingManager.Instance.IsFinalDay())
        {
            oldBorders.SetActive(false);
            newBorders.SetActive(true);
            constructionSite.SetActive(false);
            exitPortal.SetActive(false); //can't go back home
        }
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
}
