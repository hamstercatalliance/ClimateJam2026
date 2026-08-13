using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
public class FinalDayEndingManager : MonoBehaviour
{
    private const string DOWNTOWN_SCENE_NAME = "Downtown";
    private const string HOME_SCENE_NAME = "Home";
    public static FinalDayEndingManager Instance { get; private set; }

    [SerializeField] private GameObject gloomyEffect;
    [SerializeField] private GameObject rain;
    [SerializeField] private GameObject dayUI;

    [SerializeField] private AudioClip rainSounds;
    [SerializeField] private GameObject doorInteractable;

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
        StartCoroutine(LateStart());
    }
    private IEnumerator LateStart()
    {
        yield return null;

        if (IsFinalDay())
        {
            dayUI.SetActive(false);
            HowToPlay.Instance.HideHelpButton();
            QuestNotifyManager.Instance.HideQuestNotification();
            
            if (gameObject.scene.name == HOME_SCENE_NAME)
            {
                doorInteractable.SetActive(false);
            }

            if (!SympathyPointsManager.Instance.HasReachedGoodEndingThreshold())
            {
                Debug.Log("Final day, but not enough sympathy points for a good end.");

                gloomyEffect.SetActive(true);
                rain.SetActive(true);

                AudioSource audioSource = SoundManager.Instance.GetAudioSource();
                audioSource.clip = rainSounds;
                audioSource.loop = true;
                audioSource.Play();

                if (gameObject.scene.name == DOWNTOWN_SCENE_NAME)
                {
                    FinalDaySceneManager.Instance.BadEnd();
                }
                yield break;
            }
            else
            {
                Debug.Log("Final day and enough sympathy points for a good end.");
                if (gameObject.scene.name == DOWNTOWN_SCENE_NAME)
                {
                    FinalDaySceneManager.Instance.GoodEnd();
                }
            }
        }
        Debug.Log("Not the final day or not a good end.");
        gloomyEffect.SetActive(false);
        rain.SetActive(false);
    }
    public bool IsFinalDay()
    {
        return GameData.Instance.DayManagerDayCount == DayCountdown.Instance.GetCountdownDays();
    }

}
