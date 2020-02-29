using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DisasterUIController : MonoBehaviour
{
    [System.Serializable]
    class DisasterContent
    {
        public CanvasGroup canvasGroup = null;
        public DisasterIcon disasterIcon = null;
    }

    [SerializeField]
    private DisasterManager disasterManager = null;

    [SerializeField]
    private DisasterContent disasterContent = null;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Flash;

    private void Start()
    {
        HideDirectDisasterContent();

        disasterManager.AddDisasterEventListener(OnDisaster);
        disasterManager.AddNoDisasterEventListener(OnNoDisaster);
    }

    void OnDisaster(Disaster disaster)
    {
        disasterContent.disasterIcon.SetDisaster(disaster);
        ShowDisasterContent();
    }

    void OnNoDisaster()
    {
        disasterContent.disasterIcon.SetDisaster(null);
        HideDisasterContent();
    }

    void ShowDisasterContent()
    {
        disasterContent.canvasGroup.DOFade(1f, tweenDuration).SetEase(tweenEase);
    }

    void HideDisasterContent()
    {
        disasterContent.canvasGroup.DOFade(0f, tweenDuration).SetEase(tweenEase);
    }

    private void HideDirectDisasterContent()
    {
        disasterContent.canvasGroup.DOFade(0f, 0f);
    }
}
