using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoWebSwitch : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public string videoFileName = "EndDay.mp4";

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

#if UNITY_WEBGL && !UNITY_EDITOR
        string webglPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = webglPath;
#else
        string desktopPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = desktopPath;
#endif

        // Pre-loads the first frame of the video and caches it in memory
        videoPlayer.Prepare();
    }

    // Call this public method from a UI Button click or another script
    public void StartMyVideo()
    {
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            // If the video isn't fully prepared yet, listen for completion then play
            videoPlayer.prepareCompleted += PlayAfterPrepared;
        }
    }

    private void PlayAfterPrepared(VideoPlayer vp)
    {
        videoPlayer.prepareCompleted -= PlayAfterPrepared; // Unsubscribe to avoid memory leaks
        vp.Play();
    }
}
