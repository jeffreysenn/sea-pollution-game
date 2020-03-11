using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DisasterUIController : MonoBehaviour
{
    /*
     * DisasterUIController: based on DisasterManager events, shows or hides the corresponding icon (if there is a disaster or not)
     *  - also loads corresponding VideoClip in VideoLoader when shown, and shows and hides the video when the icon is clicked
     * 
     * debug: default video clip for each disaster and default
     */

    [System.Serializable]
    class DisasterContent
    {
        public CanvasGroup canvasGroup = null;
        public DisasterIcon disasterIcon = null;
    }
    
    private DisasterManager disasterManager = null;

    [SerializeField]
    private DisasterContent defaultContent = null;

    [SerializeField]
    private DisasterContent disasterContent = null;

    [SerializeField]
    private WorldWindow worldWindow = null;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Flash;

    [Header("Debug")]
    [SerializeField]
    private bool isDebug = false;
    [SerializeField]
    private string defaultVideoClip = "World_Farm_Fine_Rabbit";

    private DisasterContent currentContentShown = null;

    private void Start()
    {
        disasterManager = UIManager.Instance.disasterManager;

        if (isDebug)
        {
            Disaster tempDefaultDisaster = new Disaster();
            tempDefaultDisaster.clipTitle = defaultVideoClip;
            defaultContent.disasterIcon.SetDisaster(tempDefaultDisaster);
        }

        defaultContent.disasterIcon.OnClick += DisasterIcon_OnClick;
        disasterContent.disasterIcon.OnClick += DisasterIcon_OnClick;

        worldWindow.videoLoader.OnClipFinish += VideoLoader_OnClipFinish;

        HideDirectContent(disasterContent);

        ShowContent(defaultContent);

        disasterManager.AddDisasterEventListener(OnDisaster);
        disasterManager.AddNoDisasterEventListener(OnNoDisaster);
    }

    private void OnDestroy()
    {
        defaultContent.disasterIcon.OnClick -= DisasterIcon_OnClick;
        disasterContent.disasterIcon.OnClick -= DisasterIcon_OnClick;

        worldWindow.videoLoader.OnClipFinish -= VideoLoader_OnClipFinish;
    }


    private void ShowContent(DisasterContent content)
    {
        content.canvasGroup.DOFade(1f, tweenDuration).SetEase(tweenEase);
        content.canvasGroup.blocksRaycasts = true;

        currentContentShown = content;

        worldWindow.videoLoader.LoadVideo(content.disasterIcon.GetDisaster().clipTitle);
    }

    private void HideContent(DisasterContent content)
    {
        content.canvasGroup.DOFade(0f, tweenDuration).SetEase(tweenEase);
        content.canvasGroup.blocksRaycasts = false;
    }

    private void HideDirectContent(DisasterContent content)
    {
        content.canvasGroup.DOFade(0f, 0f);
        content.canvasGroup.blocksRaycasts = false;
    }

    // callbacks

    private void OnDisaster(Disaster disaster)
    {
        if (isDebug)
        {
            disaster.clipTitle = defaultVideoClip;
        }

        disasterContent.disasterIcon.SetDisaster(disaster);

        HideContent(defaultContent);
        ShowContent(disasterContent);
    }

    private void OnNoDisaster()
    {
        if (currentContentShown == disasterContent)
        {
            HideContent(disasterContent);
            ShowContent(defaultContent);

        }

        disasterContent.disasterIcon.SetDisaster(null);
    }

    private void DisasterIcon_OnClick(DisasterIcon obj)
    {
        if (worldWindow.IsVideoShown())
        {
            worldWindow.HideVideo();
        }
        else
        {
            if (disasterContent.disasterIcon == obj)
            {

                worldWindow.ShowVideo(true);
            } else
            {
                worldWindow.ShowVideo();
            }

        }
    }

    private void VideoLoader_OnClipFinish()
    {
        if(worldWindow.IsVideoShown())
        {
            worldWindow.HideVideo();
        } else
        {

        }
    }
}
