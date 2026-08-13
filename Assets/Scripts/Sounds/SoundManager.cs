using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour, IHasPersistentData
{
    //this will eventully be used to play SFX and adjust SFX volume
    private AudioSource audioSource;
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
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
        audioSource = GetComponent<AudioSource>();
    }
    public bool DataSuccessfullyWritten { get; private set; } = false;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip jumpSound;

    [SerializeField] private AudioClip[] grassWalkingSounds;
    [SerializeField] private AudioClip[] stoneWalkingSounds;
    [SerializeField] private AudioClip[] woodWalkingSounds;
    [SerializeField] private AudioClip[] sandWalkingSounds;

    [SerializeField] private AudioClip questInitiatedSound;
    [SerializeField] private AudioClip questCompletedSound;

    [SerializeField] private AudioClip menuOpenSound;
    [SerializeField] private AudioClip menuCloseSound;

    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip clickSound;

    [SerializeField] private AudioClip transactionSound;
    [SerializeField] private AudioClip enterStoreSound;
    [SerializeField] private AudioClip exitStoreSound;

    [SerializeField] private AudioClip itemDiscardedSound;
    [SerializeField] private AudioClip vanityEquippedSound;

    [SerializeField] private AudioClip boardOpenSound;
    [SerializeField] private AudioClip boardCloseSound;

    [SerializeField] private AudioClip endOfDayBellSound;
    [SerializeField] private AudioClip nightAmbianceSound;

    [SerializeField] private AudioClip castFishingRodSound;
    [SerializeField] private AudioClip fishSplashSound;
    [SerializeField] private AudioClip reelFishSound;


    private float footstepsVolumeMultiplier = 0.1f;
    private float jumpVolumeMultiplier = 0.4f;
    
    void Start()
    {
        SceneLoader.OnSceneTransition += OnSceneTransitionHandler;
        LoadGameData();

        //listening to siound triggering events
        Quest.OnQuestInitiated += Quest_OnQuestInitiated;
        Quest.OnQuestCompleted += Quest_OnQuestCompleted;
        MenuManager.Instance.OnMenuOpened += MenuManager_OnMenuOpened;
        MenuManager.Instance.OnMenuClosed += MenuManager_OnMenuClosed;
        Player.Instance.OnPickup += Player_OnPickup;
        Click.OnClick += Click_OnClick;
        MerchantStore.OnTransaction += MerchantStore_OnTransaction;
        MerchantStore.OnStoreEntered += MerchantStore_OnStoreEntered;
        MerchantStore.OnStoreExited += MerchantStore_OnStoreExited;
        InventoryManager.Instance.OnItemDiscarded += InventoryManager_OnItemDiscarded;
        VanityManager.Instance.OnVanityItemChanged += VanityManager_OnVanityItemChanged;
        BoardManager.Instance.OnBoardOpened += BoardManager_OnBoardOpened;
        BoardManager.Instance.OnBoardClosed += BoardManager_OnBoardClosed;
        DayEndCountdown.OnCountdownStarted += DayEndCountdown_OnCountdownStarted;
        Player.Instance.OnPlayerJump += Player_OnPlayerJump;
        DialogueRenderer.OnDialogueProceed += DialogueRenderer_OnDialogueProceed;
        FishingSignalHandler.OnFishHooked += FishingSignalHandler_OnFishHooked;
        FishingSignalHandler.OnRodCasted += FishingSignalHandler_OnRodCasted;
        FishingSignalHandler.OnFishReeled += FishingSignalHandler_OnFishReeled;
    }
    private void OnDestroy()
    {
        SceneLoader.OnSceneTransition -= OnSceneTransitionHandler;

        Quest.OnQuestInitiated -= Quest_OnQuestInitiated;
        Quest.OnQuestCompleted -= Quest_OnQuestCompleted;
        MenuManager.Instance.OnMenuOpened -= MenuManager_OnMenuOpened;
        MenuManager.Instance.OnMenuClosed -= MenuManager_OnMenuClosed;
        Player.Instance.OnPickup -= Player_OnPickup;
        Click.OnClick -= Click_OnClick;
        MerchantStore.OnTransaction -= MerchantStore_OnTransaction;
        MerchantStore.OnStoreEntered -= MerchantStore_OnStoreEntered;
        MerchantStore.OnStoreExited -= MerchantStore_OnStoreExited;
        InventoryManager.Instance.OnItemDiscarded -= InventoryManager_OnItemDiscarded;
        VanityManager.Instance.OnVanityItemChanged -= VanityManager_OnVanityItemChanged;
        BoardManager.Instance.OnBoardOpened -= BoardManager_OnBoardOpened;
        BoardManager.Instance.OnBoardClosed -= BoardManager_OnBoardClosed;
        DayEndCountdown.OnCountdownStarted -= DayEndCountdown_OnCountdownStarted;
        Player.Instance.OnPlayerJump -= Player_OnPlayerJump;
        DialogueRenderer.OnDialogueProceed -= DialogueRenderer_OnDialogueProceed;
        FishingSignalHandler.OnRodCasted -= FishingSignalHandler_OnRodCasted;
        FishingSignalHandler.OnFishHooked -= FishingSignalHandler_OnFishHooked;
        FishingSignalHandler.OnFishReeled -= FishingSignalHandler_OnFishReeled;
    }
    public void PlayFootstepsSound(PlayerSounds.SurfaceType surfaceType = PlayerSounds.SurfaceType.Stone)
    {
        AudioClip[] footstepSounds = null;
        switch (surfaceType)
        {
            case PlayerSounds.SurfaceType.Grass:
                footstepSounds = grassWalkingSounds;
                break;
            case PlayerSounds.SurfaceType.Stone:
                footstepSounds = stoneWalkingSounds;
                break;
            case PlayerSounds.SurfaceType.Wood:
                footstepSounds = woodWalkingSounds;
                break;
            case PlayerSounds.SurfaceType.Sand:
                footstepSounds = sandWalkingSounds;
                break;
            default:
                footstepSounds = stoneWalkingSounds; // Default
                break;
        }
            PlaySound(footstepSounds, audioSource.volume * footstepsVolumeMultiplier); 
    }
    private IEnumerator ReelThenAddToInverntory()
    {
        float reelClipLength = reelFishSound.length;
        audioSource.PlayOneShot(reelFishSound);
        yield return new WaitForSeconds(reelClipLength);
        audioSource.PlayOneShot(pickupSound);

    }
    #region Event Handlers
    private void FishingSignalHandler_OnRodCasted(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(castFishingRodSound, audioSource.volume);
    }
    private void FishingSignalHandler_OnFishHooked(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(fishSplashSound, audioSource.volume);
    }
    private void FishingSignalHandler_OnFishReeled(object sender, EventArgs e)
    {
        StartCoroutine(ReelThenAddToInverntory());
    }
    private void DialogueRenderer_OnDialogueProceed(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(clickSound, audioSource.volume);
    }
    private void Player_OnPlayerJump(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(jumpSound, audioSource.volume * jumpVolumeMultiplier);
    }
    private void DayEndCountdown_OnCountdownStarted(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(endOfDayBellSound, audioSource.volume);
    }
    private void BoardManager_OnBoardOpened(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(boardOpenSound, audioSource.volume);
    }
    private void BoardManager_OnBoardClosed(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(boardCloseSound, audioSource.volume);
    }
    private void VanityManager_OnVanityItemChanged(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(vanityEquippedSound, audioSource.volume);
    }
    private void InventoryManager_OnItemDiscarded(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(itemDiscardedSound, audioSource.volume);
    }
    private void MerchantStore_OnStoreExited(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(exitStoreSound, audioSource.volume);
    }
    private void MerchantStore_OnStoreEntered(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(enterStoreSound, audioSource.volume);
    }
    private void MerchantStore_OnTransaction(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(transactionSound, audioSource.volume);
    }
    private void Click_OnClick(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(clickSound, audioSource.volume);
    }
    private void Player_OnPickup(object sender, Player.OnPickupEventArgs e)
    {
        audioSource.PlayOneShot(pickupSound, audioSource.volume);
    }
    private void Quest_OnQuestInitiated(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(questInitiatedSound, audioSource.volume);
    }
    private void Quest_OnQuestCompleted(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(questCompletedSound, audioSource.volume);
    }
    private void MenuManager_OnMenuOpened(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(menuOpenSound, audioSource.volume);
    }
    private void MenuManager_OnMenuClosed(object sender, EventArgs e)
    {
        audioSource.PlayOneShot(menuCloseSound, audioSource.volume);
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
    private void PlaySound(AudioClip[] audioClipArray)
    {
        audioSource.PlayOneShot(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)]);
    }
    private void PlaySound(AudioClip[] audioClipArray, float volume = 1f)
    {
        audioSource.PlayOneShot(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], volume);
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
