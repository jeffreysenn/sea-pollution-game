using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HowToPlayMenu : MonoBehaviour
{
    [SerializeField]
    private Button btnContinue = null;

    [SerializeField]
    private CanvasGroup menuCanvas = null;
    [SerializeField]
    private float tweenDuration = 0.5f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    public event Action OnContinue;

    private void Start()
    {
        btnContinue.onClick.AddListener(OnClick_Continue);
    }

    void OnClick_Continue()
    {
        OnContinue?.Invoke();
    }
    
    public void Show()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
        menuCanvas.blocksRaycasts = true;
        menuCanvas.interactable = true;
    }

    public void Hide()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

        menuCanvas.blocksRaycasts = false;
        menuCanvas.interactable = false;
    }

    public void HideDirect()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(0f, 0f).SetEase(tweenEase);

        menuCanvas.blocksRaycasts = false;
        menuCanvas.interactable = false;
    }
}
