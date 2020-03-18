using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContent : PopUpPieChartContent
{
    public bool CheckFlow(Flow flow)
    {
        bool hasFoundData = false;
        
        if (flow != null)
        {
            PollutionMap map = flow.GetPollutionMap();

            float total = map.GetTotalPollution();

            if (total < 0)
            {
                hasFoundData = true;

                SetPieChart(pieChart, Util.MultiplyMap(map, -1));
            }
            else if (total > 0)
            {
                hasFoundData = true;

                SetPieChart(pieChart, map);
            }

            imageToShow = false;
            imageIsDisaster = false;
        }

        return hasFoundData;
    }

    public bool CheckNode(Node node)
    {
        bool hasFoundData = false;

        if (node != null)
        {
            PollutionMap map = node.GetPollutionMap();

            float total = map.GetTotalPollution();

            if (total < 0)
            {
                hasFoundData = true;

                SetPieChart(pieChart, Util.MultiplyMap(map, -1));
            }
            else if (total > 0)
            {
                hasFoundData = true;

                SetPieChart(pieChart, map);
            }

            imageToShow = false;
            imageIsDisaster = false;
        }

        return hasFoundData;
    }
}
