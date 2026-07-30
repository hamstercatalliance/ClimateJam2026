using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicUIManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.OnMusicVolumeChanged += MusicManager_OnMusicVolumeChanged;
    }
    private void OnEnable()
    {
        MusicManager_OnMusicVolumeChanged(this, new MusicManager.MusicVolumeChangedEventArgs
        {
            newVolume = MusicManager.Instance.GetComponent<AudioSource>().volume
        });
    }
    private void MusicManager_OnMusicVolumeChanged(object sender, MusicManager.MusicVolumeChangedEventArgs e)
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
        MusicManager.Instance.OnMusicVolumeChanged -= MusicManager_OnMusicVolumeChanged;
    }
}
