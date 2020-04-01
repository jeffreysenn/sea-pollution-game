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
    [SerializeField]
    private string defaultText = "No system shock";

    private Disaster disaster;

    public event Action<DisasterIcon> OnClick;

    public void SetDisaster(Disaster d) {
        disaster = d;

        if(d == null)
        {
            disasterText.text = defaultText;
        } else
        {
            if (d.title == "")
                disasterText.text = defaultText;
            else
                disasterText.text = d.title;
        }


    }
    public Disaster GetDisaster() { return disaster; }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
