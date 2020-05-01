using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPieChart : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    struct PollutionMapTypeWithTransform
    {
        public PollutionMapType pollutionMapType;
        public RectTransform targetTransform;
    }

    [SerializeField]
    private PieChartController pollutionPie = null;
    [SerializeField]
    private List<PollutionMapTypeWithTransform> showingOrder = new List<PollutionMapTypeWithTransform>();

    WorldStateManager worldStateManager = null;

    private Player player = null;

    private int currentTypeShown = 0;

    public void Activate()
    {
        worldStateManager = UIManager.Instance.worldStateManager;

        if (player == null) { Debug.LogError("[PlayerPieChart] Active: Player not found"); return; }

        var playerState = worldStateManager.GetPlayerState(player.id);
        foreach (var stateChangeEvent in playerState.GetPollutionChangeEvents())
        {
            stateChangeEvent.AddListener(UpdateCurrentPieChart);
        }

        UpdateCurrentPieChart();

        //pollutionPie.OnPieChartClick += OnPieChartClick;
    }

    private void UpdateCurrentPieChart()
    {
        PollutionMap map = worldStateManager.GetPlayerState(player.id).GetTurnPollutionMap(showingOrder[currentTypeShown].pollutionMapType);

        if (showingOrder[currentTypeShown].pollutionMapType == PollutionMapType.FILTERED)
        {
            map = Util.MultiplyMap(map, (-1));
        }

        pollutionPie.SetPollutionMap(map);

        pollutionPie.Draw();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //OnClick();
    }

    private void OnPieChartClick(PieChartController pcc)
    {
        //OnClick();
    }

    public void OnClick()
    {
        showingOrder[currentTypeShown].targetTransform.gameObject.SetActive(false);

        currentTypeShown = (currentTypeShown + 1) % showingOrder.Count;

        showingOrder[currentTypeShown].targetTransform.gameObject.SetActive(true);

        UpdateCurrentPieChart();
    }

    public void SetPlayer(Player p) { player = p; }
}
