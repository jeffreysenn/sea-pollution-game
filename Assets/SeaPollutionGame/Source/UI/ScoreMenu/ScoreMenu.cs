using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreMenu : MonoBehaviour
{
    [SerializeField]
    private CustomBarChart scoreBar = null;

    [Header("Details")]
    [SerializeField]
    private CustomBarChart resourcesChart = null;
    [SerializeField]
    private CustomBarChart pollutionChart = null;
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

        scoreBar.SetLeftValue(50);

        defaultDetailsPosition = detailsTransform.anchoredPosition;

        scoreBar.OnClick += ScoreBar_OnClick;
    }
    
    private void Update()
    {
        UpdateScore();

        if(isShown)
        {
            UpdateResources();
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
        float player1Score = worldStateManager.GetScore(player1.id);
        float player2Score = worldStateManager.GetScore(player2.id);
        float ratioScore = (player1Score / (player1Score + player2Score)) * 100;

        if ((player1Score + player2Score == 0))
        {
            ratioScore = 50;
        }

        if (player1Score == Mathf.Infinity || player2Score == Mathf.Infinity)
        {
            ratioScore = 50;
        }

        scoreBar.SetLeftValue(ratioScore);
    }

    private void UpdateResources()
    {
        float player1Resources = player1State.GetMoney();
        float player2Resources = player2State.GetMoney();
        float ratioResources = (player1Resources / (player1Resources + player2Resources)) * 100;

        if (player1Resources + player2Resources == 0)
        {
            ratioResources = 50;
        }

        resourcesChart.SetLeftValue(ratioResources);
    }

    private void UpdatePollution()
    {
        float player1Pollution = player1State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();
        float player2Pollution = player2State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();

        float ratioPollution = (player1Pollution / (player1Pollution + player2Pollution)) * 100;

        if (player1Pollution + player2Pollution == 0)
        {
            ratioPollution = 50;
        }

        pollutionChart.SetLeftValue(ratioPollution);
    }

    private void UpdateFiltered()
    {
        float player1Filtered = player1State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();
        float player2Filtered = player2State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();

        float ratioFiltered = (player1Filtered / (player1Filtered + player2Filtered)) * 100;

        if (player1Filtered + player2Filtered == 0)
        {
            ratioFiltered = 50;
        }

        filteredChart.SetLeftValue(ratioFiltered);
    }

    private void UpdateEfficiency()
    {
        float player1Efficiency = worldStateManager.GetEfficiency(player1.id);
        float player2Efficiency = worldStateManager.GetEfficiency(player2.id);
        float ratioEfficiency = (player1Efficiency / (player1Efficiency + player2Efficiency)) * 100;

        if ((player1Efficiency + player2Efficiency == 0))
        {
            ratioEfficiency = 50;
        }

        if (player1Efficiency == Mathf.Infinity || player2Efficiency == Mathf.Infinity)
        {
            ratioEfficiency = 50;
        }

        efficiencyChart.SetLeftValue(ratioEfficiency);
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
