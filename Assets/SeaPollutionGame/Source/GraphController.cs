using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;

public class GraphController : MonoBehaviour
{
    [SerializeField] private GraphChart graph = null;
    [SerializeField] private Image background = null;
    private WorldStateManager worldStateManager = null;
    private Dictionary<int, StateHistory> histories = new Dictionary<int, StateHistory> { };

    private void Start()
    {
        worldStateManager = FindObjectOfType<WorldStateManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            graph.gameObject.SetActive(!graph.gameObject.activeInHierarchy);
            background.gameObject.SetActive(!background.gameObject.activeInHierarchy);
        }

        if (graph.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Plot(PlayerStateBlackboard.Key.MONEY);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                Plot(PlayerStateBlackboard.Key.ASSET_VALUE);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                Plot(PlayerStateBlackboard.Key.POLLUTION, PollutionMapType.NET, "Emission");
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                Plot(PlayerStateBlackboard.Key.RESOURCE, "SPG 13");

        }
    }

    public void SetStateHistory(int id, StateHistory history) { histories[id] = history; }

    public void Plot(PlayerStateBlackboard.Key key)
    {
        Plot(blackboard => blackboard.GetValue(key));
    }

    public void Plot(PlayerStateBlackboard.Key key, PollutionMapType pollutionMapType, string pollutionName)
    {
        Plot(blackboard =>
        {
            var val = blackboard.GetValue(key, pollutionMapType, pollutionName);
            val = val < 0 ? -val : val;
            return val;
        });
    }

    public void Plot(PlayerStateBlackboard.Key key, string resourceName)
    {
        Plot(blackboard => blackboard.GetValue(key, resourceName));
    }

    private void Plot(Func<PlayerStateBlackboard, float> getValueFuc)
    {
        graph.DataSource.StartBatch();
        var pids = worldStateManager.GetPlayerIDs();
        foreach (var playerID in pids)
        {
            var category = "Player " + playerID.ToString();
            graph.DataSource.ClearCategory(category);
            var balckboard = worldStateManager.GetStateHistory(playerID);
            for (int i = 0; i != balckboard.Count; ++i)
            {
                float val = getValueFuc.Invoke(balckboard[i]);
                graph.DataSource.AddPointToCategory(category, i, val);
            }
        }
        graph.DataSource.EndBatch();
    }

}
