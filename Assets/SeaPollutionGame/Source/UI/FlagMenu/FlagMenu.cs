﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class FlagMenu : MonoBehaviour
{
    [SerializeField]
    private List<FlagIcon> flagsPrefab = null;
    [SerializeField]
    private RectTransform targetTransform = null;
    [SerializeField]
    private CanvasGroup menuCanvas = null;
    [SerializeField]
    private Button btnStart = null;

    [SerializeField]
    private GameObject countrySelected = null;
    [SerializeField]
    private TextMeshProUGUI countrySelectedText = null;

    [SerializeField]
    private float tweenDuration = 0.5f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private List<FlagIcon> flags = null;
    private FlagIcon flagSelected = null;

    private void Start()
    {
        countrySelected.SetActive(false);

        GenerateFlags();

        btnStart.onClick.AddListener(BtnStart_OnClick);

        Show();
    }

    private void OnDestroy()
    {
        btnStart.onClick.RemoveListener(BtnStart_OnClick);
        foreach(FlagIcon icon in flags)
        {
            icon.OnClick -= FlagIcon_OnClick;
        }
    }

    private void GenerateFlags()
    {
        List<FlagIcon> tempFlags = new List<FlagIcon>(flagsPrefab);

        Util.Shuffle(tempFlags);

        flags = new List<FlagIcon>();
        foreach(FlagIcon icon in tempFlags)
        {
            FlagIcon temp = Instantiate(icon, targetTransform);
            temp.OnClick += FlagIcon_OnClick;
            flags.Add(temp);
        }

    }

    private void FlagIcon_OnClick(FlagIcon obj)
    {
        foreach(FlagIcon icon in flags)
        {
            if(icon == obj)
            {
                flagSelected = icon;
                icon.Select();
            } else
            {
                icon.Deselect();
            }
        }
    }

    private void BtnStart_OnClick()
    {
        countrySelectedText.text = GetSelectedFlag().ToString();
        countrySelected.SetActive(true);

        Hide();
    }

    public FlagType GetSelectedFlag() {

        if(flagSelected == null) { Debug.LogError("[FlagMenu] GetSelectedFlag: no flag selected");}

        return flagSelected.GetFlagType();
    }

    private void Show()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
        menuCanvas.blocksRaycasts = true;
        menuCanvas.interactable = true;
    }

    private void Hide()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

        menuCanvas.blocksRaycasts = false;
        menuCanvas.interactable = false;
    }

    private void HideDirect()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(0f, 0f).SetEase(tweenEase);

        menuCanvas.blocksRaycasts = false;
        menuCanvas.interactable = false;
    }
}