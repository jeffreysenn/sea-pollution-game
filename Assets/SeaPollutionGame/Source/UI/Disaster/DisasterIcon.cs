using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class DisasterIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string defaultText = "No system shock";

    private Disaster disaster;

    public event Action<DisasterIcon> OnClick;

    public string GetTitle()
    {
        if (disaster == null) return defaultText;
        return disaster.title;
    }

    public string GetDescription()
    {
        if (disaster == null) return "";
        return disaster.description;
    }

    public void SetDisaster(Disaster d) {
        disaster = d;
    }

    public Disaster GetDisaster() { return disaster; }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
