using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polluter : MonoBehaviour
{
    protected PolluterAttrib polluterAttrib = null;
    protected WorldStateManager stateManager = null;

    int ownerID = -1;

    public void SetOwnerID(int id) { ownerID = id; }
    public int GetOwnerID() { return ownerID; }

    public void SetAttrib(PolluterAttrib attrib) { polluterAttrib = attrib; }

    public PolluterAttrib GetAttrib() { return polluterAttrib; }

    protected void Start()
    {
        stateManager = WorldStateManager.FindWorldStateManager();
    }

    protected void MakeMoney()
    {
        var profit = polluterAttrib.economicAttrib.profitPerTurn;
        stateManager.AddMoney(ownerID, profit);
    }

    protected void Pollute()
    {
        foreach (var emission in polluterAttrib.pollutionAttrib.emissions)
        {
            stateManager.AddPollution(ownerID, emission.emissionPerTurn);
        }
    }

    public void Purchase()
    {
        float price = polluterAttrib.economicAttrib.price;
        stateManager.AddMoney(ownerID, -price);
    }

    public virtual void Activate()
    {
        SetOwnerID(stateManager.GetCurrentPlayerID());
        Purchase();
    }
}
