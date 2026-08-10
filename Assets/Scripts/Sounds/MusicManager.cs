using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour, IHasPersistentData
{
    private string TOWN_SCENE = "Downtown";
    private AudioSource audioSource;
    public event EventHandler<MusicVolumeChangedEventArgs> OnMusicVolumeChanged;
    public class MusicVolumeChangedEventArgs : EventArgs
    {
        public float newVolume;
    }
    public static MusicManager Instance { get; private set; }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public bool DataSuccessfullyWritten { get; private set; } = false;
    void Start()
    {
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        LoadGameData();

        if (FinalDayEndingManager.Instance.IsFinalDay() && gameObject.scene.name == TOWN_SCENE)
        {
            audioSource.clip = null; //null or assign a specific AudioClip for the final town scene
        }
    }
    private void OnSceneTransitionHandler(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;
    }
    public void IncreaseVolume(float amount = 0.2f)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + amount, 0f, 1f);
        OnMusicVolumeChanged?.Invoke(this, new MusicVolumeChangedEventArgs 
        { 
            newVolume = audioSource.volume
        });
    }
    public void DecreaseVolume(float amount = 0.2f)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume - amount, 0f, 1f);
        OnMusicVolumeChanged?.Invoke(this, new MusicVolumeChangedEventArgs 
        { 
            newVolume = audioSource.volume
        });
    }

    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            audioSource.volume = GameData.Instance.MusicVolume;
            OnMusicVolumeChanged?.Invoke(this, new MusicVolumeChangedEventArgs 
            { 
                newVolume = audioSource.volume
            });
        }
        else
        {
            audioSource.volume = 1f; // Default volume if no game data is available
            OnMusicVolumeChanged?.Invoke(this, new MusicVolumeChangedEventArgs 
            { 
                newVolume = audioSource.volume
            });
        }
    }
    public void WriteToGameData()
    {
        GameData.Instance.MusicVolume = audioSource.volume;
        DataSuccessfullyWritten = true;
    }
}
