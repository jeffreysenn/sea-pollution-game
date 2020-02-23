using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWorldStats : MonoBehaviour
{
    WorldStateManager stateManager = null;
    PieController[] pieControllers = new PieController[3];
    PollutionMap[] pollutionMaps = new PollutionMap[3];


    void Start()
    {
        stateManager = GetComponent<WorldStateManager>();
        stateManager.AddEndPlayerTurnFinishEventListener(UpdateWorldStats);
        pieControllers[0] = GameObject.Find("ProducedPollutionPie").GetComponent<PieController>();
        pieControllers[1] = GameObject.Find("FilteredPollutionPie").GetComponent<PieController>();
        pieControllers[2] = GameObject.Find("NetPollutionPie").GetComponent<PieController>();
        UpdateWorldStats();
    }

    void UpdateWorldStats()
    {
        for (int i = 0; i != 3; ++i)
        {
            var type = (PollutionMapType)i;
            var map = stateManager.GetPollutionMapSum(type);
            pieControllers[i].SetPollutionMap(map);
            pieControllers[i].Draw();
        }
    }
}
