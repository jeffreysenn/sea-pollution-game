using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalticContent : PopUpPieChartContent
{


    public bool CheckBalticSea(Node node, string balticTag)
    {
        bool hasFoundData = false;
        
        if (node != null)
        {
            if (node.CompareTag(balticTag))
            {
                hasFoundData = true;

                PollutionMap map = node.GetPollutionMap();

                SetPieChart(pieChart, map);
            }

            imageToShow = false;
            imageIsDisaster = false;
        }

        return hasFoundData;
    }
}
