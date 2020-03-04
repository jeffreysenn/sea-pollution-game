using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;


public class DisasterIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI disasterText = null;

    private Disaster disaster;

    public event Action<DisasterIcon> OnClick;

    public void SetDisaster(Disaster d) {
        disaster = d;

        if (d == null)
            disasterText.text = "";
        else
            disasterText.text = d.title;
    }
    public Disaster GetDisaster() { return disaster; }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
