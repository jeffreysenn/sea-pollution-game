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

    private void Awake()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if (worldStateManager == null) { Debug.LogError("[PlayerPieChart] Start: WorldStateManager not found"); return; }

    }

    public void Activate()
    {
        if(player==null) { Debug.LogError("[PlayerPieChart] Active: Player not found"); return; }

        worldStateManager.AddEndPlayerTurnFinishEventListener(UpdateCurrentPieChart);

        pollutionPie.SetPollutionMap(worldStateManager.GetPollutionMap(player.id, showingOrder[currentTypeShown].pollutionMapType));

    }

    private void UpdateCurrentPieChart()
    {
        PollutionMap map = worldStateManager.GetPollutionMap(player.id, showingOrder[currentTypeShown].pollutionMapType);

        if(showingOrder[currentTypeShown].pollutionMapType == PollutionMapType.FILTERED)
        {
            map = Util.MultiplyMap(map, (-1));
        }

        pollutionPie.SetPollutionMap(map);

        pollutionPie.Draw();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        showingOrder[currentTypeShown].targetTransform.gameObject.SetActive(false);

        currentTypeShown = (currentTypeShown + 1) % showingOrder.Count;

        showingOrder[currentTypeShown].targetTransform.gameObject.SetActive(true);

        UpdateCurrentPieChart();
    }

    public void SetPlayer(Player p) { player = p; }
}
