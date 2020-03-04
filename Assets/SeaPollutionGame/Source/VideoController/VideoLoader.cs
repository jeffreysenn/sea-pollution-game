using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class VideoLoader : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer = null;

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
        VideoClip clip = Resources.Load<VideoClip>(videoName) as VideoClip;

        if(clip == null) { Debug.LogError("[VideoLoader] LoadVideo: clip doesn't exist: " + videoName); return; }

        videoPlayer.clip = clip;

        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
        if(videoPlayer.clip == null) { Debug.LogWarning("[VideoLoader] PlayVideo: clip not loaded"); return; }
        
        videoPlayer.Play();
    }

    public void StopVideo()
    {
        if (videoPlayer.clip == null) { Debug.LogWarning("[VideoLoader] StopVideo: clip not loaded"); return; }

        videoPlayer.Stop();
    }
}
