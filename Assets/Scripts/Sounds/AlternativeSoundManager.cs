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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        Scene scene = gameObject.scene;
        if (scene.name == MENU_SCENE)
        {
            Debug.Log("Playing click sound in menu scene");
            audioSource.PlayOneShot(click, 1f);
        }
        else if (scene.name == SAVE_SCENE)
        {
            float volume = GameData.Instance.SoundVolume;
            audioSource.PlayOneShot(click, volume);
        }
        else
        {
            Debug.Log("??");
        }
    }
}
