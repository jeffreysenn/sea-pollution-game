using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Polluter : MonoBehaviour
{
    protected PolluterAttrib polluterAttrib = null;
    protected string description = "";
    [SerializeField] protected Text descriptionText = null;

    int ownerID = -1;

    public void SetOwnerID(int id) { ownerID = id; }

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
