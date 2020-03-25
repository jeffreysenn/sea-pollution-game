using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
    [SerializeField]
    private CustomBarChart scoreBar = null;
    [SerializeField]
    private bool percentageValues = true;

    [Header("Details")]
    [SerializeField]
    private CustomBarChart resourcesChart = null;
    [SerializeField]
    private CustomBarChart totalChart = null;
    [SerializeField]
    private CustomBarChart producedChart = null;
    [SerializeField]
    private CustomBarChart filteredChart = null;
    [SerializeField]
    private CustomBarChart efficiencyChart = null;
    [SerializeField]
    private CustomBarChart assetValueChart = null;

    [Header("Tween")]
    [SerializeField]
    private RectTransform detailsTransform = null;
    [SerializeField]
    private Vector2 detailsTargetPosition = Vector2.zero;
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private Vector2 defaultDetailsPosition = Vector2.zero;

    WorldStateManager worldStateManager = null;

    Player player1 = null; PlayerState player1State = null;
    Player player2 = null; PlayerState player2State = null;

    public event Action<ScoreMenu> OnClick;

    private bool _isShown = false;
    public bool isShown { get { return _isShown; } }

    private void Start()
    {
        worldStateManager = UIManager.Instance.worldStateManager;

        player1 = UIManager.Instance.player1;
        player2 = UIManager.Instance.player2;

        player1State = worldStateManager.GetPlayerState(player1.id);
        player2State = worldStateManager.GetPlayerState(player2.id);

        scoreBar.SetValues(50, 50, true);

        defaultDetailsPosition = detailsTransform.anchoredPosition;

        scoreBar.OnClick += ScoreBar_OnClick;
    }
    
    private void Update()
    {
        UpdateScore();

        if(_isShown)
        {
            UpdateResources();
            UpdateTotal();
            UpdatePollution();
            UpdateFiltered();
            UpdateEfficiency();
            UpdateAssetValue();
        }
    }

    private void ScoreBar_OnClick(CustomBarChart chart)
    {
        OnClick?.Invoke(this);
    }

    private void UpdateScore()
    {
        float p1 = worldStateManager.GetScore(player1.id);
        float p2 = worldStateManager.GetScore(player2.id);

        
        scoreBar.SetValues(p1, p2, percentageValues);
    }

    private void UpdateResources()
    {
        float p1 = player1State.GetMoney();
        float p2 = player2State.GetMoney();

        
        resourcesChart.SetValues(p1, p2, percentageValues);
    }

    private void UpdateTotal()
    {
        float p1 = player1State.GetAccumulatedPollutionMap(PollutionMapType.NET).GetTotalPollution();
        float p2 = player2State.GetAccumulatedPollutionMap(PollutionMapType.NET).GetTotalPollution();
        
        
        totalChart.SetValues(p1, p2, percentageValues);
    }

    private void UpdatePollution()
    {
        float p1 = player1State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();
        float p2 = player2State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();

        
        producedChart.SetValues(p1, p2, percentageValues);
    }

    private void UpdateFiltered()
    {
        float p1 = player1State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();
        float p2 = player2State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();

        if(p1 <= 0 && p2 <= 0)
        {
            p1 *= -1;
            p2 *= -1;
        }
        
        filteredChart.SetValues(p1, p2, percentageValues);
    }

    private void UpdateEfficiency()
    {
        float p1 = worldStateManager.GetEfficiency(player1.id);
        float p2 = worldStateManager.GetEfficiency(player2.id);

        
        efficiencyChart.SetValues(p1, p2, percentageValues);
    }

    private void UpdateAssetValue()
    {
        float p1 = player1State.GetAssetValue();
        float p2 = player2State.GetAssetValue();

        assetValueChart.SetValues(p1, p2, percentageValues);
    }

    public void Show()
    {
        if (_isShown) return;

        _isShown = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(detailsTransform);

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(detailsTargetPosition, tweenDuration).SetEase(tweenEase);
    }

    public void Hide()
    {
        if (!_isShown) return;

        _isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, tweenDuration).SetEase(tweenEase);
    }

    public void HideDirect()
    {
        _isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, 0f).SetEase(tweenEase);
    }
}
