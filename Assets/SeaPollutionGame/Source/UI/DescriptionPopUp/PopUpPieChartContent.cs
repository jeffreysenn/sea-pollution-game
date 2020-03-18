using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpPieChartContent : PopUpContent
{
    public PieChartController pieChart = null;

    public void SetPieChart(PieChartController pieChart, PollutionMap map)
    {
        pieChart.Clear();
        pieChart.SetPollutionMap(map);
        pieChart.Draw();
    }
}
