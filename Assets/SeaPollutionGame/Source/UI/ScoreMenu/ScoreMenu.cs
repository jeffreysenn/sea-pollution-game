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
        float player1Score = worldStateManager.GetScore(player1.id);
        float player2Score = worldStateManager.GetScore(player2.id);

        scoreBar.SetLeftValue(NormalizedRatio(player1Score, player2Score));
    }

    private void UpdateResources()
    {
        float player1Resources = player1State.GetMoney();
        float player2Resources = player2State.GetMoney();

        resourcesChart.SetLeftValue(NormalizedRatio(player1Resources, player2Resources));
    }

    private void UpdateTotal()
    {
        float player1Total = player1State.GetAccumulatedPollutionMap(PollutionMapType.NET).GetTotalPollution();
        float player2Total = player2State.GetAccumulatedPollutionMap(PollutionMapType.NET).GetTotalPollution();
        
        totalChart.SetLeftValue(NormalizedRatio(player1Total, player2Total));
    }

    private void UpdatePollution()
    {
        float player1Pollution = player1State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();
        float player2Pollution = player2State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();


        producedChart.SetLeftValue(NormalizedRatio(player1Pollution, player2Pollution));
    }

    private void UpdateFiltered()
    {
        float player1Filtered = player1State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();
        float player2Filtered = player2State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();

        filteredChart.SetLeftValue(NormalizedRatio(player1Filtered, player2Filtered));
    }

    private void UpdateEfficiency()
    {
        float player1Efficiency = worldStateManager.GetEfficiency(player1.id);
        float player2Efficiency = worldStateManager.GetEfficiency(player2.id);

        efficiencyChart.SetLeftValue(NormalizedRatio(player1Efficiency, player2Efficiency));
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

    private float NormalizedRatio(float v1, float v2)
    {
        float value1 = v1;
        float value2 = v2;
        if (value1 < 0)
        {
            value1 *= -1;

            if (value2 < 0)
            {
                value2 *= -1;
            }
            else
            {
                value2 += value1;
            }
        }

        if (value2 < 0)
        {
            value2 *= -1;

            if (value2 < 0)
            {
                value2 *= -1;
            }
            else
            {
                value1 += value2;
            }

        }

        float ratio = (value1 / (value1 + value2)) * 100;
        //Debug.Log("\t" + value1 + " " + value2 + " " + ratio + " (" + (value1 + value2) + ")");

        if ((value1 + value2 == 0))
        {
            ratio = 50;
        }

        if (value1 == Mathf.Infinity || value2 == Mathf.Infinity)
        {
            Debug.LogWarning("sanity infinity");
            ratio = 50;
        }

        return ratio;
    }
}
