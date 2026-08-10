using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class AlternativeSoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip click;

    private string MENU_SCENE = "Menu";
    private string SAVE_SCENE = "SaveScene";
    private string INTRO_SCENE = "Intro";
    private string GOOD_END_SCENE = "GoodEnd";
    private string BAD_END_SCENE = "BadEnd";

    public static AlternativeSoundManager Instance { get; private set; }
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
        audioSource = GetComponent<AudioSource>();
        Slideshow.OnProceed += Slideshow_OnProceed;
    }
    private void Slideshow_OnProceed(object sender, EventArgs e)
    {
        PlayClickSound();
    }
    private void OnDestroy()
    {
        Slideshow.OnProceed -= Slideshow_OnProceed;
    }

    public void PlayClickSound()
    {
        Scene scene = gameObject.scene;
        if (scene.name == MENU_SCENE)
        {
            Debug.Log("Playing click sound in menu scene");
            audioSource.PlayOneShot(click, 1f);
        }
        else if (scene.name == SAVE_SCENE || scene.name == INTRO_SCENE || scene.name == GOOD_END_SCENE || scene.name == BAD_END_SCENE)
        {
            float volume = GameData.Instance.SoundVolume;
            audioSource.PlayOneShot(click, volume);
        }
        else
        {
            Debug.Log("not supposed to fall through");
        }
    }
}
