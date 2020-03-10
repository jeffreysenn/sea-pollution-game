using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayersStatController : MonoBehaviour
{
    [System.Serializable]
    class PlayerStat
    {
        public PlayerState playerState { get; set; }
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

    private void Start()
    {
        player1Stat.player = UIManager.Instance.player1;
        player2Stat.player = UIManager.Instance.player2;

        worldStateManager = UIManager.Instance.worldStateManager;
        worldStateManager.AddEndPlayerTurnFinishEventListener(UpdateEndTurn);

        player1Stat.playerState = worldStateManager.GetPlayerState(player1Stat.player.id);
        player2Stat.playerState = worldStateManager.GetPlayerState(player2Stat.player.id);

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
        PollutionMap map1 = player1Stat.playerState.GetTurnPollutionMap(typeOrder[currentTypeIndex].mapType);
        float value1 = map1.GetTotalPollution();

        PollutionMap map2 = player2Stat.playerState.GetTurnPollutionMap(typeOrder[currentTypeIndex].mapType);
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
        UpdateCoins(player1Stat.txtCoinsValue, player1Stat.playerState.GetMoney());
        UpdateCoins(player2Stat.txtCoinsValue, player2Stat.playerState.GetMoney());

        UpdateIncome(player1Stat.txtIncomeValue, player1Stat.playerState.GetTurnIncome());
        UpdateIncome(player2Stat.txtIncomeValue, player2Stat.playerState.GetTurnIncome());
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
