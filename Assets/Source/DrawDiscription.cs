using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDiscription : MonoBehaviour
{
    [SerializeField] Text descriptionText = null;

    string description = "";
    void Start()
    {
        var polluter = GetComponent<Polluter>();
        var attrib = polluter.GetAttrib();
        description = attrib.GetDescription();
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
