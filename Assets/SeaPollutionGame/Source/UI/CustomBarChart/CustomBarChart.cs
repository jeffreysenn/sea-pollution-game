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
        float max = 100;

        if(d < 0)
        {
            d *= -1;
            max += d;
        }

        if(d > 100) {
            Debug.LogWarning("[CustomBarChart] SetLeftValue: " + d + " is above 100");
            return;
        }

        float value = d;
        if (value < threshold) value = threshold;
        if (value > max - threshold) value = max - threshold;

        float leftValue = (widthTotalContent * value) / max;

        float rightValue = (widthTotalContent * (max - value)) / max;

        leftContent.sizeDelta = new Vector2((float) leftValue, leftContent.sizeDelta.y);
        rightContent.sizeDelta = new Vector2((float) rightValue, leftContent.sizeDelta.y);

        leftText.text = Math.Round(d, textDecimal).ToString() + textSuffixe;
        rightText.text = Math.Round((100 - d), textDecimal).ToString() + textSuffixe;
    }

    /*
    public void SetRightValue(float d)
    {
        SetLeftValue(100 - d);
    }
    */

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
