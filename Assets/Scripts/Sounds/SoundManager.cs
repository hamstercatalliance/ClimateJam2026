using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SoundManager : MonoBehaviour, IHasPersistentData
{
    //this will eventully be used to play SFX and adjust SFX volume
    private AudioSource audioSource;
    public event EventHandler<VolumeChangedEventArgs> OnVolumeChanged;
    public class VolumeChangedEventArgs : EventArgs
    {
        public float newVolume;
    }
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public bool DataSuccessfullyWritten { get; private set; } = false;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] grassWalkingSounds;
    [SerializeField] private AudioClip[] stoneWalkingSounds;
    [SerializeField] private AudioClip questInitiatedSound;
    [SerializeField] private AudioClip questCompletedSound;
    [SerializeField] private AudioClip menuOpenSound;
    [SerializeField] private AudioClip menuCloseSound;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        LoadGameData();

        //listening to siound triggering events
        Quest.OnQuestInitiated += Quest_OnQuestInitiated;
        Quest.OnQuestCompleted += Quest_OnQuestCompleted;
        MenuManager.Instance.OnMenuOpened += MenuManager_OnMenuOpened;
        MenuManager.Instance.OnMenuClosed += MenuManager_OnMenuClosed;
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;

        Quest.OnQuestInitiated -= Quest_OnQuestInitiated;
        Quest.OnQuestCompleted -= Quest_OnQuestCompleted;
        MenuManager.Instance.OnMenuOpened -= MenuManager_OnMenuOpened;
        MenuManager.Instance.OnMenuClosed -= MenuManager_OnMenuClosed;
    }
    #region Event Handlers
    private void Quest_OnQuestInitiated(object sender, EventArgs e)
    {
        PlaySound(questInitiatedSound, Camera.main.transform.position, audioSource.volume);
    }
    private void Quest_OnQuestCompleted(object sender, EventArgs e)
    {
        PlaySound(questCompletedSound, Camera.main.transform.position, audioSource.volume);
        Debug.Log(sender.ToString() + " completed quest sound played");
    }
    private void MenuManager_OnMenuOpened(object sender, EventArgs e)
    {
        PlaySound(menuOpenSound, Camera.main.transform.position, audioSource.volume);
    }
    private void MenuManager_OnMenuClosed(object sender, EventArgs e)
    {
        PlaySound(menuCloseSound, Camera.main.transform.position, audioSource.volume);
    }
    #endregion

    #region Saving and Loading
    private void OnSceneTransitionHandler(object sender, EventArgs e)
    {
        WriteToGameData();
    }
    public void LoadGameData()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData)
        {
            audioSource.volume = GameData.Instance.SoundVolume;
            OnVolumeChanged?.Invoke(this, new VolumeChangedEventArgs 
            { 
                newVolume = audioSource.volume
            });
        }
        else
        {
            audioSource.volume = 1f; // Default volume if no game data is available
            OnVolumeChanged?.Invoke(this, new VolumeChangedEventArgs 
            { 
                newVolume = audioSource.volume
            });
        }
    }
    public void WriteToGameData()
    {
        GameData.Instance.SoundVolume = audioSource.volume;
        DataSuccessfullyWritten = true;
    }
    #endregion

    #region Play sound methods
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }
    #endregion

    #region Volume methods
    public void IncreaseVolume(float amount = 0.2f)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + amount, 0f, 1f);
        OnVolumeChanged?.Invoke(this, new VolumeChangedEventArgs 
        { 
            newVolume = audioSource.volume
        });
    }
    public void DecreaseVolume(float amount = 0.2f)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume - amount, 0f, 1f);
        OnVolumeChanged?.Invoke(this, new VolumeChangedEventArgs 
        { 
            newVolume = audioSource.volume
        });
    }
    #endregion
    
}
