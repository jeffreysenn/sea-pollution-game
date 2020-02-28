using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatController : MonoBehaviour
{
    /*
     * TODO: Add Player Income data
     *       Add event when money of a player changes
     *       Add event when pollutionmap of a type changes
     *       Change "tweenYOffsetInitial" to get value from Start
     */

    WorldStateManager worldStateManager = null;

    [SerializeField]
    private Player player = null;

    [Header("Header")]
    [SerializeField]
    private TextMeshProUGUI txtCoinsValue = null;
    [SerializeField]
    private TextMeshProUGUI txtIncome = null;

    [Header("Pie Charts")]
    [SerializeField]
    private PieChartController totalPollutionPie = null;
    [SerializeField]
    private PieChartController producedPollutionPie = null;
    [SerializeField]
    private PieChartController filteredPollutionPie = null;

    [Header("Details")]
    [SerializeField]
    private RectTransform contentDetails = null;
    [SerializeField]
    private Button btnDetails = null;
    [SerializeField]
    private float tweenYOffset = 0f;
    [SerializeField]
    private float tweenYOffsetInitial = 0f;
    [SerializeField]
    private float tweenDuration = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private bool isContentDetailsShown = false;

    private void Start()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if (worldStateManager == null) { Debug.LogError("[PlayerStatController] Start: WorldStateManager not found"); return; }

        txtCoinsValue.text = worldStateManager.GetMoney(player.id).ToString();
        //UpdateIncome(worldStateManager.GetIncome(player.id));

        //totalPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.NET));

        worldStateManager.AddEndPlayerTurnFinishEventListener(UpdatePieCharts);

        totalPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.NET));
        producedPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.PRODUCED));
        filteredPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.FILTERED));
        
        btnDetails.onClick.AddListener(OnBtnDetailsClick);
    }

    private void OnDestroy()
    {
        btnDetails.onClick.RemoveListener(OnBtnDetailsClick);
    }

    void UpdatePieCharts()
    {
        totalPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.NET));
        producedPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.PRODUCED));
        filteredPollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, PollutionMapType.FILTERED));

        totalPollutionPie.Draw();
        producedPollutionPie.Draw();
        filteredPollutionPie.Draw();
    }

    private void Update()
    {
        txtCoinsValue.text = worldStateManager.GetMoney(player.id).ToString();
    }

    void OnBtnDetailsClick()
    {
        if(!isContentDetailsShown)
        {
            contentDetails.DOLocalMoveY(tweenYOffset, tweenDuration).SetEase(tweenEase);
        } else
        {
            contentDetails.DOLocalMoveY(tweenYOffsetInitial, tweenDuration).SetEase(tweenEase);
        }

        isContentDetailsShown = !isContentDetailsShown;
    }

    void UpdateIncome(int value)
    {
        string s = "(";
        if(value > 0)
        {
            s += "+" + value;
        } else
        {
            s += value;
        }
        s += ")";

        txtIncome.text = s;
    }
}
