using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDescription : MonoBehaviour
{
    [SerializeField] public Text descriptionText = null;

    protected string description = "";

    protected void OnMouseEnter()
    {
        if (descriptionText)
        {
            descriptionText.text = description;
        }
    }

    protected void OnMouseExit()
    {
        if (descriptionText)
        {
            descriptionText.text = "";
        }
    }
}
