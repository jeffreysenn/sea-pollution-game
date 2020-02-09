using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Polluter : MonoBehaviour
{
    protected PolluterAttrib polluterAttrib = null;
    protected string description = "";
    [SerializeField] protected Text descriptionText = null;

    protected void Start()
    {
        description = polluterAttrib.GetDescription();
    }

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
