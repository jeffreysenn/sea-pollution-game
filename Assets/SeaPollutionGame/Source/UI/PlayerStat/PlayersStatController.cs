using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayersStatController : MonoBehaviour
{
    [System.Serializable]
    class PlayerStat
    {
        public TextMeshProUGUI txtCoinsValue;
        public TextMeshProUGUI txtIncomeValue;
        public Player player { get; set; }
    }

    [System.Serializable]
    class MapType
    {
        public PollutionMapType mapType;
        public string title;
    }

    [SerializeField]
    private PlayersPieChart pieChart = null;

    [SerializeField]
    private PlayerStat player1Stat;
    [SerializeField]
    private PlayerStat player2Stat;

    [SerializeField]
    private List<MapType> typeOrder = null;
    private int currentTypeIndex = 0;

    private WorldStateManager worldStateManager = null;

    private void Awake()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if (worldStateManager == null) { Debug.LogError("[PlayersStatController] Start: WorldStateManager not found"); return; }
    }

    private void Start()
    {
        player1Stat.player = UIManager.Instance.player1;
        player2Stat.player = UIManager.Instance.player2;
        
        worldStateManager.AddEndPlayerTurnFinishEventListener(UpdateEndTurn);

        pieChart.OnClick += PieChart_OnClick;
    }

    private void Update()
    {
        UpdateValues();
    }

    private void PieChart_OnClick(PlayersPieChart chart)
    {
        currentTypeIndex = (currentTypeIndex + 1) % typeOrder.Count;
        UpdatePieChart();
    }

    private void UpdateEndTurn()
    {
        UpdatePieChart();
        UpdateValues();
    }

    private void UpdatePieChart()
    {
        PollutionMap map1 = worldStateManager.GetPollutionMap(player1Stat.player.id, typeOrder[currentTypeIndex].mapType);
        float value1 = map1.GetTotalPollution();

        PollutionMap map2 = worldStateManager.GetPollutionMap(player2Stat.player.id, typeOrder[currentTypeIndex].mapType);
        float value2 = map2.GetTotalPollution();

        if (typeOrder[currentTypeIndex].mapType == PollutionMapType.FILTERED)
        {
            value1 *= -1;
            value2 *= -1;
        }

        pieChart.SetPlayersValue(value1, value2);
        pieChart.SetTitle(typeOrder[currentTypeIndex].title);

        pieChart.Draw();
    }

    private void UpdateValues()
    {
        UpdateCoins(player1Stat.txtCoinsValue, worldStateManager.GetMoney(player1Stat.player.id));
        UpdateCoins(player2Stat.txtCoinsValue, worldStateManager.GetMoney(player2Stat.player.id));

        UpdateIncome(player1Stat.txtIncomeValue, worldStateManager.GetIncome(player1Stat.player.id));
        UpdateIncome(player2Stat.txtIncomeValue, worldStateManager.GetIncome(player2Stat.player.id));
    }

    private void UpdateCoins(TextMeshProUGUI txt, float value)
    {
        txt.text = value.ToString();
    }

    private void UpdateIncome(TextMeshProUGUI txt, float value)
    {
        if (value == 0) { txt.text = ""; return; }

        string s = "(";
        if (value > 0)
        {
            s += "+" + value;
        }
        else
        {
            s += value;
        }
        s += ")";

        txt.text = s;
    }
}
