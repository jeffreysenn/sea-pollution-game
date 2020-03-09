using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class PlayersPieChart : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    class PlayerStatData
    {
        public Color color = Color.white;
        public Material material { get; set; }
        public float value { get; set; }
        public string categoryName = "Player";
    }

    [SerializeField]
    private PlayerStatData player1;
    [SerializeField]
    private PlayerStatData player2;

    [SerializeField]
    private PieChart pieChart = null;
    [SerializeField]
    private PieAnimation pieAnimation = null;
    [SerializeField]
    private TextMeshProUGUI textSum = null;
    [SerializeField]
    private TextMeshProUGUI textTitle = null;

    public event Action<PlayersPieChart> OnClick;

    private void Start()
    {
        Clear();
        Setup();
    }

    private void Setup()
    {
        player1.material = new Material(Shader.Find("Chart/Canvas/Solid"));
        player1.material.color = player1.color;

        player2.material = new Material(Shader.Find("Chart/Canvas/Solid"));
        player2.material.color = player2.color;
    }

    public void SetTitle(string s)
    {
        if (textTitle)
            textTitle.text = s;
    }

    public void SetPlayersValue(float p1, float p2)
    {
        player1.value = p1;
        player2.value = p2;
    }

    public void Draw()
    {
        Clear();

        pieChart.DataSource.AddCategory(player1.categoryName, player1.material);
        pieChart.DataSource.AddCategory(player2.categoryName, player2.material);

        pieChart.DataSource.SetValue(player1.categoryName, player1.value);
        pieChart.DataSource.SetValue(player2.categoryName, player2.value);

        if(pieAnimation)
            pieAnimation.Animate();

        if (textSum)
            textSum.text = Math.Round(player1.value + player2.value, 1).ToString();
    }

    private void Clear()
    {
        pieChart.DataSource.Clear();

        if (textSum)
            textSum.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
