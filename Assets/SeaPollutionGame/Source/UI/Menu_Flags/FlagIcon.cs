using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using TMPro;

public class FlagIcon : MonoBehaviour, IPointerClickHandler
{
    public event Action<FlagIcon> OnClick;

    [SerializeField]
    private CountryType flagType = CountryType.SWEDEN;

    [SerializeField]
    private CanvasGroup selectedCanvas = null;

    [SerializeField]
    private TextMeshProUGUI txtTitle = null;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip onClickClip = null;

    private bool isSelected = false;

    public bool isClickable = true;

    private void Start()
    {
        string flagText = flagType.ToString();
        txtTitle.text = flagText[0].ToString().ToUpper() + flagText.Substring(1).ToLower();

        HideDirectSelected();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isClickable)
            OnClick?.Invoke(this);
    }

    public void Select()
    {
        if (isSelected) return;

        isSelected = true;

        selectedCanvas.DOKill();
        selectedCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);

        audioSource.clip = onClickClip;
        audioSource.Play();
    }

    public void Deselect()
    {
        if (!isSelected) return;

        isSelected = false;

        selectedCanvas.DOKill();
        selectedCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

        audioSource.Stop();
    }

    public void HideDirectSelected()
    {
        isSelected = false;

        selectedCanvas.DOKill();
        selectedCanvas.DOFade(0f, 0f).SetEase(tweenEase);
    }

    public void ShowText()
    {
        txtTitle.gameObject.SetActive(true);
    }

    public void HideText()
    {
        txtTitle.gameObject.SetActive(false);
    }

    public CountryType GetFlagType() { return flagType; }
}
