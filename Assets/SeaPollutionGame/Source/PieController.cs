using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;

public class PieController : MonoBehaviour
{
    PollutionMap pollutionMap = null;
    PieChart pie = null;
    PieAnimation pieAnimation = null;
    PollutantMaterialManager materialManager = null;
    Text sum = null;

    public void SetPollutionMap(PollutionMap map) { pollutionMap = map; }

    void Start()
    {
        pie = GetComponent<PieChart>();
        if (pie)
        {
            pie.DataSource.Clear();
        }
        pieAnimation = GetComponent<PieAnimation>();
        materialManager = FindObjectsOfType<PollutantMaterialManager>()[0];
        sum = GetComponentInChildren<Text>();
        if (sum)
        {
            sum.text = "";
        }
    }

    public void Draw()
    {
        if (pollutionMap != null && pie && materialManager)
        {
            pie.DataSource.Clear();
            foreach (var pair in pollutionMap)
            {
                var mat = materialManager.GetMaterial(pair.Key);
                pie.DataSource.AddCategory(pair.Key, mat);
                pie.DataSource.SetValue(pair.Key, pair.Value);
            }
            if (pieAnimation)
            {
                pieAnimation.Animate();
            }
            if (sum)
            {
                sum.text = pollutionMap.GetTotalPollution().ToString();
            }
        }
    }

    public void Clear()
    {
        if (pie)
        {
            pie.DataSource.Clear();
        }
        if (sum)
        {
            sum.text = "";
        }
    }

}
