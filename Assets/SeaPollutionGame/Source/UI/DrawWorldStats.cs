using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWorldStats : MonoBehaviour
{
    WorldStateManager stateManager = null;
    PieController[] pieControllers = new PieController[3];


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
            var map = new PollutionMap(stateManager.GetPollutionMapSum(type));
            if(type == PollutionMapType.FILTERED)
            {
                map = Util.MultiplyMap(map, -1);
            }
            pieControllers[i].SetPollutionMap(map);
            pieControllers[i].Draw();
        }
    }
}
