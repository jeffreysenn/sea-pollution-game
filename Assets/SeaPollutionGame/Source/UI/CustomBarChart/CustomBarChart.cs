using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class CustomBarChart : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private RectTransform totalContent = null;
    [SerializeField]
    private RectTransform leftContent = null;
    [SerializeField]
    private RectTransform rightContent = null;

    [SerializeField]
    private TextMeshProUGUI leftText = null;
    [SerializeField]
    private TextMeshProUGUI rightText = null;
    [SerializeField]
    private string textSuffixe = "%";
    [SerializeField]
    private int textDecimal = 0;

    [SerializeField]
    private int threshold = 10;

    private float widthTotalContent = 1;

    public event Action<CustomBarChart> OnClick;

    private void Awake()
    {
        widthTotalContent = totalContent.sizeDelta.x;
    }

    public void SetLeftValue(float d)
    {
        if(d < 0 || d > 100) { Debug.LogWarning("[CustomBarChart] SetLeftValue: " + d + " is not between 0 and 100"); return; }

        float value = d;
        if (value < threshold) value = threshold;
        if (value > 100 - threshold) value = 100 - threshold;

        float leftValue = (widthTotalContent * value) / 100;

        float rightValue = (widthTotalContent * (100 - value)) / 100;

        leftContent.sizeDelta = new Vector2((float) leftValue, leftContent.sizeDelta.y);
        rightContent.sizeDelta = new Vector2((float) rightValue, leftContent.sizeDelta.y);

        leftText.text = Math.Round(d, textDecimal).ToString() + textSuffixe;
        rightText.text = Math.Round((100 - d), textDecimal).ToString() + textSuffixe;
    }

    public void SetRightValue(float d)
    {
        SetLeftValue(100 - d);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
