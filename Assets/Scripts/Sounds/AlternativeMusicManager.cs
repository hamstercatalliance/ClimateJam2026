using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AlternativeMusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private string GOOD_END_SCENE = "GoodEnd";
    private string BAD_END_SCENE = "BadEnd";
    private string INTRO_SCENE = "Intro";

    [Header("Intro only")]
    [SerializeField] private AudioSource intro1Player;
    [SerializeField] private AudioSource intro2Player;
    void Start()
    {
        if (gameObject.scene.name == INTRO_SCENE)
        {
            intro1Player.volume = GameData.Instance.MusicVolume;
            intro2Player.volume = 0f;
            return;
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameData.Instance.MusicVolume;
    }

    public void FadeTransitionToIntro2()
    {
        StartCoroutine(FadeOutAudioSource(intro2Player, intro1Player, 1f));
    }

    private IEnumerator FadeOutAudioSource(AudioSource audioSourceIn, AudioSource audioSourceOut, float fadeDuration)
    {
        //turns up the volume of in and down out
        float baseVolume = GameData.Instance.MusicVolume;
        float dt = 0f;
        while (dt < fadeDuration)
        {
            dt += Time.deltaTime;
            float t = dt / fadeDuration;
            audioSourceIn.volume = Mathf.Lerp(0f, baseVolume, t);
            audioSourceOut.volume = Mathf.Lerp(baseVolume, 0f, t);
            yield return null;
        }
    }
}
