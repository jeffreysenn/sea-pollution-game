using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisasterContent : PopUpContent
{
    [SerializeField]
    private TextMeshProUGUI textTitle = null;

    public bool CheckGraphicDisaster(DisasterIcon disasterIcon)
    {
        bool hasFoundData = false;

        Disaster disaster = disasterIcon.GetDisaster();
        if (disaster != null)
        {
            hasFoundData = true;

            textTitle.text = disaster.title;
        }

        imageToShow = false;

        return hasFoundData;
    }
}
