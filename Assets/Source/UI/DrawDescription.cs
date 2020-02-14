using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDescription : MonoBehaviour
{
    [SerializeField] public Text descriptionText = null;

    protected string description = "";

    public void SetDescription(string des) { description = des; }

    protected void OnMouseEnter()
    {
        descriptionText = GameObject.Find("ItemDescription").GetComponent<Text>();

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
