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

    public void SetPollutionMap(PollutionMap map) { pollutionMap = map; }

    void Start()
    {
        pie = GetComponent<PieChart>();
        if (pie)
        {
            pie.DataSource.Clear();
        }
        pieAnimation = GetComponent<PieAnimation>();
    }

    public void Draw()
    {
        if (pollutionMap != null && pie)
        {
            pie.DataSource.Clear();
            int counter = 0;
            foreach (var pair in pollutionMap)
            {
                pie.DataSource.AddCategory(pair.Key, materials[counter++]);
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
