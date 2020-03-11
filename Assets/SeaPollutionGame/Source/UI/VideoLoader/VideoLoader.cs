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
    [SerializeField]
    private string filePath = "Clips/";

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
        //videoPlayer.url = "Assets/Resources/" + videoName + clipExtension;
        VideoClip clip = Resources.Load<VideoClip>(filePath + videoName) as VideoClip;

        if (clip == null) { Debug.LogError("[VideoLoader] LoadVideo: clip doesn't exist: " + videoName); return; }

        videoPlayer.clip = clip;
        
        videoPlayer.Prepare();
    }

    public void PlayVideo()
    {
        if(videoPlayer.clip == null) { Debug.LogWarning("[VideoLoader] PlayVideo: clip not found"); return; }

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
        if(videoPlayer.clip == null) { Debug.LogWarning("[VideoLoader] StopVideo: clip not found"); return; }

        videoPlayer.Stop();
    }
}
