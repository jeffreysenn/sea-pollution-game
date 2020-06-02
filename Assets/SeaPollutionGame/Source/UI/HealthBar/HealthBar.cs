using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text = null;
    [SerializeField]
    private Image background = null;

    private float value = 0f;

    private Health healthComp = null;

    private void HealthBar_OnHealthModified(float value)
    {
        SetValuePercentage(value);
    }

    public void SetValuePercentage(float v)
    {
        if (v < 0f || v > 100f) return;

        value = v;

        text.text = v + "%";
        
        Color newColor = new Color(1, 1, v/100f, 1);

        background.color = newColor;
    }

    public void Show(Health h)
    {
        healthComp = h;

        gameObject.SetActive(true);

        if(healthComp != null)
        {
            SetValuePercentage(healthComp.GetHealth());
            healthComp.OnHealthModified += HealthBar_OnHealthModified;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if(healthComp != null)
            healthComp.OnHealthModified -= HealthBar_OnHealthModified;

        healthComp = null;
    }
}
