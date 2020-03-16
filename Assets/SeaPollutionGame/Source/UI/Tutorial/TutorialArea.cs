using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialArea : MonoBehaviour
{
    [SerializeField]
    private PopUpOrientation _popUpOrientation = PopUpOrientation.LEFT;
    public PopUpOrientation popUpOrientation { get { return _popUpOrientation; } }

    [SerializeField]
    private CanvasGroup _canvasGroup = null;
    public CanvasGroup canvasGroup { get { return _canvasGroup; } }

    [SerializeField]
    private string _title = "title";
    public string title { get { return _title; } }

    [SerializeField]
    [TextArea(3, 10)]
    private string _description = "description";
    public string description { get { return _description; } }

    [Header("Tween")]
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private bool isShown = false;

    public void Show()
    {
        if (isShown) return;

        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, tweenDuration).SetEase(tweenEase);

        canvasGroup.blocksRaycasts = true;

        isShown = true;
    }

    public void Hide()
    {
        if (!isShown) return;

        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, tweenDuration).SetEase(tweenEase);

        canvasGroup.blocksRaycasts = false;

        isShown = false;
    }

    public void HideDirect()
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, 0f);

        canvasGroup.blocksRaycasts = false;

        isShown = false;
    }
}
