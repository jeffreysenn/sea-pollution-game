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

    [System.Serializable]
    struct PollutionMapTypeWithTransform
    {
        public PollutionMapType pollutionMapType;
        public RectTransform targetTransform;
    }

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
    private PieChartController pollutionPie = null;
    [SerializeField]
    private List<PollutionMapTypeWithTransform> showingOrder = new List<PollutionMapTypeWithTransform>();


    private bool isContentDetailsShown = false;
    private int currentTypeShown = 0;

    private void Start()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if (worldStateManager == null) { Debug.LogError("[PlayerStatController] Start: WorldStateManager not found"); return; }

        UpdateCoins(worldStateManager.GetMoney(player.id));
        //UpdateIncome(worldStateManager.GetIncome(player.id));

        worldStateManager.AddEndPlayerTurnFinishEventListener(UpdateCurrentPieChart);
        pollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, showingOrder[currentTypeShown].pollutionMapType));

        pollutionPie.OnPieChartClick += PieChart_OnPieChartClick;
    }

    private void OnDestroy()
    {
        pollutionPie.OnPieChartClick -= PieChart_OnPieChartClick;
    }

    private void Update()
    {
        UpdateCoins(worldStateManager.GetMoney(player.id));
    }

    private void UpdateCurrentPieChart()
    {
        Debug.Log(showingOrder.Count + " " + showingOrder[currentTypeShown]);

        pollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, showingOrder[currentTypeShown].pollutionMapType));

        pollutionPie.Draw();
    }
    
    private void PieChart_OnPieChartClick(PieChartController obj)
    {
        showingOrder[currentTypeShown].targetTransform.gameObject.SetActive(false);

        currentTypeShown += 1 % showingOrder.Count;

        showingOrder[currentTypeShown].targetTransform.gameObject.SetActive(true);

        UpdateCurrentPieChart();
    }

    private void UpdateCoins(float value)
    {
        txtCoinsValue.text = value.ToString();
    }

    private void UpdateIncome(float value)
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
