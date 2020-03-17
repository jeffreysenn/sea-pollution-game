using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    [Header("Tween")]
    [SerializeField]
    private RectTransform detailsTransform = null;
    [SerializeField]
    private Vector2 detailsTargetPosition = Vector2.zero;
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;
    [SerializeField]
    private List<PlayerStatController> playerStatControllers = null;

    private Vector2 defaultDetailsPosition = Vector2.zero;

    WorldStateManager worldStateManager = null;

    Player player1 = null; PlayerState player1State = null;
    Player player2 = null; PlayerState player2State = null;

    private bool isShown = false;

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

        if(isShown)
        {
            UpdateResources();
            UpdateTotal();
            UpdatePollution();
            UpdateFiltered();
            UpdateEfficiency();
        }
    }

    private void ScoreBar_OnClick(CustomBarChart chart)
    {
        if (isShown)
            Hide();
        else
            Show();
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

        
        filteredChart.SetValues(p1, p2, percentageValues);
    }

    private void UpdateEfficiency()
    {
        float p1 = worldStateManager.GetEfficiency(player1.id);
        float p2 = worldStateManager.GetEfficiency(player2.id);

        
        efficiencyChart.SetValues(p1, p2, percentageValues);
    }

    public void Show()
    {
        if (isShown) return;

        isShown = true;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(detailsTargetPosition, tweenDuration).SetEase(tweenEase);

        foreach(PlayerStatController psc in playerStatControllers)
        {
            psc.Hide();
        }
    }

    public void Hide()
    {
        if (!isShown) return;

        isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, tweenDuration).SetEase(tweenEase);

        foreach (PlayerStatController psc in playerStatControllers)
        {
            psc.Show();
        }
    }

    public void HideDirect()
    {
        isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, 0f).SetEase(tweenEase);

        foreach (PlayerStatController psc in playerStatControllers)
        {
            psc.Show();
        }
    }
}
