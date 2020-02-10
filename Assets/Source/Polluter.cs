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
        var emission = polluterAttrib.pollutionAttrib.emissionPerTurn;
        stateManager.AddPollution(ownerID, emission);
    }

    public void Purchase()
    {
        float price = polluterAttrib.economicAttrib.costToPurchase;
        stateManager.AddMoney(ownerID, -price);
    }

    public virtual void Activate()
    {
        SetOwnerID(stateManager.GetCurrentPlayerID());
        Purchase();
    }
}
