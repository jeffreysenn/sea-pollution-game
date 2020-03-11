using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;
using TMPro;
using System;

public class PieChartController : MonoBehaviour
{
    [SerializeField]
    private PieChart pie = null;
    [SerializeField]
    private PieAnimation pieAnimation = null;

    private PollutantMaterialManager pollutantMaterialManager = null;

    [SerializeField]
    private TextMeshProUGUI txtSum = null;

    PollutionMap pollutionMap = null;

    public event Action<PieChartController> OnPieChartClick;

    public void SetPollutionMap(PollutionMap map) {
        pollutionMap = map;
        pollutionMap.Keys.ToString();
    }

    void Start()
    {
        pollutantMaterialManager = UIManager.Instance.pollutantMaterialManager;

        Clear();
        
        pollutantMaterialManager = FindObjectOfType<PollutantMaterialManager>();

        CanvasPieChart canvasPieChart = pie as CanvasPieChart;
        canvasPieChart.PieClicked.AddListener(OnPieClick);
    }

    public void Draw()
    {
        if (pollutionMap != null && pie && pollutantMaterialManager)
        {
            pie.DataSource.Clear();
            foreach (var pair in pollutionMap)
            {
                var mat = pollutantMaterialManager.GetMaterial(pair.Key);
                pie.DataSource.AddCategory(pair.Key, mat);
                pie.DataSource.SetValue(pair.Key, pair.Value);
            }
            if (pieAnimation)
            {
                pieAnimation.Animate();
            }

            if (txtSum)
            {
                txtSum.text = Math.Round(pollutionMap.GetTotalPollution(), 1).ToString();
            }
            
        }
    }

    public void Clear()
    {
        if (pie)
        {
            pie.DataSource.Clear();
        }
        if (txtSum)
        {
            txtSum.text = "";
        }
    }

    private void OnPieClick(PieChart.PieEventArgs pieEventArgs)
    {
        OnClick();
    }

    public void OnClick()
    {
        OnPieChartClick?.Invoke(this);
    }
}
