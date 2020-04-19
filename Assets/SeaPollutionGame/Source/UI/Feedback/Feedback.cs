﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Feedback : MonoBehaviour
{
    [Header("Text Feedback")]
    [SerializeField]
    private Color errorColor = Color.red;
    [SerializeField]
    private Ease flashingEase = Ease.Linear;
    [SerializeField]
    private float flashingDuration = 0.1f;
    [SerializeField]
    private int flashingIteration = 3;


    [Header("Target insufficient coins")]
    [SerializeField]
    private PlayersStatController playersStatController = null;
    [SerializeField]
    private DescriptionPopUp descriptionPopUp = null;

    // singleton
    private static Feedback _instance;

    public static Feedback Instance { get { return _instance; } }

    WorldStateManager worldStateManager = null;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        worldStateManager = UIManager.Instance.worldStateManager;
    }

    public void FeedbackInsufficientCoins()
    {
        playersStatController.FeedbackNotEnoughCoins(worldStateManager.GetCurrentPlayerID());
        descriptionPopUp.FeedbackNotEnoughCoins();
    }

    public Sequence ErrorText(TextMeshProUGUI target, Color defaultColor)
    {
        Sequence flashingSequence = DOTween.Sequence();

        for (int i = 0; i < flashingIteration; i++)
        {
            Tween flashingTweenRed = target.DOColor(errorColor, flashingDuration).SetEase(flashingEase);
            Tween flashingTweenBack = target.DOColor(defaultColor, flashingDuration).SetEase(flashingEase);

            flashingSequence.Append(flashingTweenRed);
            flashingSequence.Append(flashingTweenBack);
        }

        return flashingSequence;
    }
}