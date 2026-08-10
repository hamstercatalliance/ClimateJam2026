using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpatialSFX : MonoBehaviour
{
    protected AudioSource audioSource;
    //I made a separate class for the water fountain because it uses spatial sound and has its own audio source
    //the sound manager is used for UI sounds and other non-spatial sounds
    //this class is to ensure that the water fountain sound is still controlled by sound manager
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        SoundManager.Instance.OnVolumeChanged += SoundManager_OnVolumeChanged;
    }
    private void OnDestroy()
    {
        SoundManager.Instance.OnVolumeChanged -= SoundManager_OnVolumeChanged;
    }
    private void SoundManager_OnVolumeChanged(object sender, SoundManager.VolumeChangedEventArgs e)
    {
        audioSource.volume = e.newVolume;
    }
}
