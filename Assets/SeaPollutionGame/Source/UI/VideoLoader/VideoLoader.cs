using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using DG.Tweening;

public class VideoLoader : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer = null;
    [SerializeField]
    private string clipExtension = ".mp4";

    public event Action OnClipFinish;

    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        OnClipFinish?.Invoke();
    }

    public void LoadVideo(string videoName)
    {
#if UNITY_WEBGL
        videoPlayer.url = "Assets/Resources/" + videoName + clipExtension;
#else
        VideoClip clip = Resources.Load<VideoClip>(videoName) as VideoClip;

        if (clip == null) { Debug.LogError("[VideoLoader] LoadVideo: clip doesn't exist: " + videoName); return; }

        videoPlayer.clip = clip;
#endif
        
        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
#if UNITY_WEBGL
        if(videoPlayer.url == "") { Debug.LogWarning("[VideoLoader] PlayVideo: clip not found"); return; }
#else
        if(videoPlayer.clip == null) { Debug.LogWarning("[VideoLoader] PlayVideo: clip not found"); return; }
#endif

        if(videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        } else
        {
            videoPlayer.Prepare();

            videoPlayer.prepareCompleted += delegate
            {
                videoPlayer.Play();
            };
        }

    }

    public void StopVideo()
    {
#if UNITY_WEBGL
        if (videoPlayer.url == "") { Debug.LogWarning("[VideoLoader] PlayVideo: clip not found"); return; }
#else
        if(videoPlayer.clip == null) { Debug.LogWarning("[VideoLoader] PlayVideo: clip not found"); return; }
#endif

        videoPlayer.Stop();
    }
}
