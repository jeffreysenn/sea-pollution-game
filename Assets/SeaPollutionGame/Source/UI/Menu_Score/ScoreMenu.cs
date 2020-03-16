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

        scoreBar.SetValuesPercentage(50);

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

        if(percentageValues)
            scoreBar.SetValuesPercentage(NormalizedRatio(p1, p2));
        else
            scoreBar.SetValues(p1, p2);
    }

    private void UpdateResources()
    {
        float p1 = player1State.GetMoney();
        float p2 = player2State.GetMoney();

        if (percentageValues)
            resourcesChart.SetValuesPercentage(NormalizedRatio(p1, p2));
        else
            resourcesChart.SetValues(p1, p2);
    }

    private void UpdateTotal()
    {
        float p1 = player1State.GetAccumulatedPollutionMap(PollutionMapType.NET).GetTotalPollution();
        float p2 = player2State.GetAccumulatedPollutionMap(PollutionMapType.NET).GetTotalPollution();
        
        if(percentageValues)
            totalChart.SetValuesPercentage(NormalizedRatio(p1, p2));
        else
            totalChart.SetValues(p1, p2);
    }

    private void UpdatePollution()
    {
        float p1 = player1State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();
        float p2 = player2State.GetAccumulatedPollutionMap(PollutionMapType.PRODUCED).GetTotalPollution();

        if (percentageValues)
            producedChart.SetValuesPercentage(NormalizedRatio(p1, p2));
        else
            producedChart.SetValues(p1, p2);
    }

    private void UpdateFiltered()
    {
        float p1 = player1State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();
        float p2 = player2State.GetAccumulatedPollutionMap(PollutionMapType.FILTERED).GetTotalPollution();

        if (percentageValues)
            filteredChart.SetValuesPercentage(NormalizedRatio(p1, p2));
        else
            filteredChart.SetValues(p1, p2);
    }

    private void UpdateEfficiency()
    {
        float p1 = worldStateManager.GetEfficiency(player1.id);
        float p2 = worldStateManager.GetEfficiency(player2.id);

        if (percentageValues)
            efficiencyChart.SetValuesPercentage(NormalizedRatio(p1, p2));
        else
            efficiencyChart.SetValues(p1, p2);
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

        if (value1 < 0) value1 = 0;
        if (value2 < 0) value2 = 0;

        float ratio = 0f;


        if(value1 == 0 && value2 == 0)
        {
            ratio = 50;
        } else
        {
            ratio = (value1) / (value1 + value2) * 100f;
        }
        
        if (value1 == Mathf.Infinity || value2 == Mathf.Infinity)
        {
            Debug.LogWarning("sanity infinity");
            ratio = 50;
        }

        return ratio;
    }
}
