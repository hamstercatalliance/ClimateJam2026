using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SoundUIManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.OnVolumeChanged += SoundManager_OnVolumeChanged;
    }
    
    private void SoundManager_OnVolumeChanged(object sender, SoundManager.VolumeChangedEventArgs e)
    {
        container.SetActive(true);
        int numBars = (int)(e.newVolume * 10) / 2; // Calculate the number of bars based on volume (0-5)
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.SetActive(i < numBars);
        }
    }
    private void OnDestroy()
    {
        SoundManager.Instance.OnVolumeChanged -= SoundManager_OnVolumeChanged;
    }
}
