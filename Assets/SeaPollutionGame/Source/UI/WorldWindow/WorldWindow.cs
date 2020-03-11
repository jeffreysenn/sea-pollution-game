using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldWindow : MonoBehaviour
{
    [SerializeField]
    private VideoLoader _videoLoader = null;
    public VideoLoader videoLoader { get { return _videoLoader; } }
    [SerializeField]
    private CanvasGroup videoCanvas = null;

    [SerializeField]
    private ImageLoader _imageLoader = null;
    public ImageLoader imageLoader { get { return _imageLoader; } }
    [SerializeField]
    private CanvasGroup imageCanvas = null;

    [SerializeField]
    private CanvasGroup content = null;

    [SerializeField]
    private GameObject alertBackground = null;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private bool isVideoShown = false;
    public bool IsVideoShown() { return isVideoShown; }
    private bool isImageShown = false;
    public bool IsImageShown() { return isImageShown; }

    private void Start()
    {
        HideDirectVideo();
        HideDirectImage();
    }

    public void SetAlert()
    {
        alertBackground.SetActive(true);
    }

    public void RemoveAlert()
    {
        alertBackground.SetActive(false);
    }

    public void ShowImage(bool isAlert = false)
    {
        if (isAlert) SetAlert();
        else RemoveAlert();

        if (!isImageShown)
        {
            content.DOKill();
            content.DOFade(1f, tweenDuration).SetEase(tweenEase);
            imageCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);

            isImageShown = true;
        }

        if(isVideoShown)
        {
            HideVideo();
        }
    }

    public void HideImage()
    {
        RemoveAlert();

        if(isImageShown)
        {
            content.DOKill();
            content.DOFade(0f, tweenDuration).SetEase(tweenEase);
            imageCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

            isImageShown = false;
        }

        if (isVideoShown)
        {
            HideVideo();
        }
    }

    public void HideDirectImage()
    {
        RemoveAlert();

        content.DOKill();
        content.DOFade(0f, 0f).SetEase(tweenEase);
        imageCanvas.DOFade(0f, 0f).SetEase(tweenEase);

        isImageShown = false;

        if (isVideoShown)
        {
            HideVideo();
        }
    }

    public void ShowVideo(bool isAlert = false)
    {
        if (isAlert) SetAlert();
        else RemoveAlert();

        if (!isVideoShown)
        {
            _videoLoader.StopVideo();

            content.DOKill();
            content.DOFade(1f, tweenDuration).SetEase(tweenEase);
            videoCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);

            isVideoShown = true;

            _videoLoader.PlayVideo();
        }

        if (isImageShown)
        {
            HideImage();
        }
    }

    public void HideVideo()
    {
        RemoveAlert();

        if (isVideoShown)
        {
            content.DOKill();
            content.DOFade(0f, tweenDuration).SetEase(tweenEase);
            videoCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

            isVideoShown = false;

            _videoLoader.StopVideo();
        }

        if (isImageShown)
        {
            HideImage();
        }
    }

    public void HideDirectVideo()
    {
        RemoveAlert();

        content.DOKill();
        content.DOFade(0f, 0f).SetEase(tweenEase);
        videoCanvas.DOFade(0f, 0f).SetEase(tweenEase);

        isVideoShown = false;

        _videoLoader.StopVideo();

        if (isImageShown)
        {
            HideImage();
        }
    }
}
