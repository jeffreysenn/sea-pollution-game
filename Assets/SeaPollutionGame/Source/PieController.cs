using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;

public class PieController : MonoBehaviour
{
    PollutionMap pollutionMap = null;
    PieChart pie = null;
    PieAnimation pieAnimation = null;
    [SerializeField] Material[] materials = null;
    PollutantMaterialManager materialManager = null;

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
        }
    }

    public void Clear()
    {
        if (pie)
        {
            pie.DataSource.Clear();
        }
    }

}
