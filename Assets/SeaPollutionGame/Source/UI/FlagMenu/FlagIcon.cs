using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class FlagIcon : MonoBehaviour, IPointerClickHandler
{
    public event Action<FlagIcon> OnClick;

    [SerializeField]
    private FlagType flagType = FlagType.SWEDEN;

    [SerializeField]
    private CanvasGroup selectedCanvas = null;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private bool isSelected = false;

    private void Start()
    {
        HideDirectSelected();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }

    public void Select()
    {
        if (isSelected) return;

        isSelected = true;

        selectedCanvas.DOKill();
        selectedCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
    }

    public void Deselect()
    {
        if (!isSelected) return;

        isSelected = false;

        selectedCanvas.DOKill();
        selectedCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);
    }

    private void HideDirectSelected()
    {
        if (!isSelected) return;

        isSelected = false;

        selectedCanvas.DOKill();
        selectedCanvas.DOFade(0f, 0f).SetEase(tweenEase);
    }

    public FlagType GetFlagType() { return flagType; }
}
