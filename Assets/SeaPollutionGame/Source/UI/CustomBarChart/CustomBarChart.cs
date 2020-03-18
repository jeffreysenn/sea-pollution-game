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
    public int GetThreshold() { return threshold; }

    private float widthTotalContent = 1;

    public event Action<CustomBarChart> OnClick;

    private void Awake()
    {
        widthTotalContent = totalContent.sizeDelta.x;
    }

    public void SetValues(float left, float right, bool percentage = true, bool withThreshold = true)
    {
        float[] values = SetLeftValue(NormalizedRatio(left, right), withThreshold);

        if (percentage)
            SetTextValues(values[0], values[1], true);
        else
            SetTextValues(left, right);
    }

    private float[] SetLeftValue(float d, bool withThreshold = true)
    {
        float max = 100;

        if(d < 0)
        {
            d *= -1;
            max += d;
        }

        /*
        if(d > 100) {
            Debug.LogWarning("[CustomBarChart] SetLeftValue: " + d + " is above 100");
            return null;
        }
        */

        float value = d;

        if(withThreshold)
        {
            if (value < threshold) value = threshold;
            if (value > max - threshold) value = max - threshold;
        }
        
        float leftValue = (widthTotalContent * value) / max;

        float rightValue = (widthTotalContent * (max - value)) / max;

        leftContent.sizeDelta = new Vector2((float) leftValue, leftContent.sizeDelta.y);
        rightContent.sizeDelta = new Vector2((float) rightValue, leftContent.sizeDelta.y);

        return new float[] { d, (100 - d) };
    }
    
    private void SetTextValues(float left, float right, bool percentage = false)
    {
        leftText.text = Math.Round(left, textDecimal).ToString();
        rightText.text = Math.Round(right, textDecimal).ToString();

        if(percentage)
        {
            leftText.text += textSuffixe;
            rightText.text += textSuffixe;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }


    private float NormalizedRatio(float v1, float v2)
    {
        float value1 = v1;
        float value2 = v2;

        if (value1 < 0) value1 = 0;
        if (value2 < 0) value2 = 0;

        float ratio = 0f;


        if (value1 == 0 && value2 == 0)
        {
            ratio = 50;
        }
        else
        {
            ratio = (value1) / (value1 + value2) * 100f;
        }

        if (value1 == Mathf.Infinity || value2 == Mathf.Infinity)
        {
            Debug.LogWarning("sanity infinity");
            ratio = 50;
        }

        return ratio;
    }
}
