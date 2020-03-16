using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialArea> tutorialAreas = new List<TutorialArea>();

    [SerializeField]
    private CanvasGroup contentCanvas = null;
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private bool isShown = false;

    private void Start()
    {
        HideDirect();
    }

    public void Show()
    {
        if (isShown) return;

        contentCanvas.DOKill();
        contentCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
        contentCanvas.blocksRaycasts = true;

        foreach(TutorialArea area in tutorialAreas)
            area.Show();

        isShown = true;
    }

    public void Hide()
    {
        if (!isShown) return;

        contentCanvas.DOKill();
        contentCanvas.DOFade(0F, tweenDuration).SetEase(tweenEase);
        contentCanvas.blocksRaycasts = false;

        foreach (TutorialArea area in tutorialAreas)
            area.Hide();

        isShown = false;
    }

    public void HideDirect()
    {
        contentCanvas.DOKill();
        contentCanvas.DOFade(0f, 0f);
        contentCanvas.blocksRaycasts = false;

        foreach (TutorialArea area in tutorialAreas)
            area.HideDirect();

        isShown = false;
    }
}
