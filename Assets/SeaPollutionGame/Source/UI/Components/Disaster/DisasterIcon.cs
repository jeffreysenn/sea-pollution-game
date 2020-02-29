using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisasterIcon : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI disasterText = null;

    private Disaster disaster;

    public void SetDisaster(Disaster d) {
        disaster = d;

        if (d == null)
            disasterText.text = "";
        else
            disasterText.text = d.title;
    }
    public Disaster GetDisaster() { return disaster; }
}
