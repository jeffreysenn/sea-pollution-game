using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Polluter : MonoBehaviour
{
    [SerializeField] protected Text descriptionText = null;
    protected PolluterAttrib polluterAttrib = null;
    protected string description = "";

    int ownerID = -1;
    WorldStateManager stateManager = null;

    public void SetOwnerID(int id) { ownerID = id; }

    protected void Start()
    {
        description = polluterAttrib.GetDescription();
        stateManager = FindObjectOfType<WorldStateManager>();
        Debug.Assert(stateManager, "World state manager must be in the scene!");
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
