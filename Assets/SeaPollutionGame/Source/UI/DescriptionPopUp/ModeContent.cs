using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeContent : PopUpContent
{
    [SerializeField]
    private TextMeshProUGUI textDescription = null;


    public bool CheckGraphicMode(ModeToggle modeToggle)
    {
        bool hasFoundData = false;

        if (modeToggle != null)
        {
            hasFoundData = true;

            textDescription.text = modeToggle.GetDescription();
        }

        imageToShow = false;

        return hasFoundData;
    }
}
