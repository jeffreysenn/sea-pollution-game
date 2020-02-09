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

    public PolluterAttrib GetAttrib() { return polluterAttrib; }

    protected void Start()
    {
        stateManager = FindObjectOfType<WorldStateManager>();
        Debug.Assert(stateManager, "World state manager must be in the scene!");
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


}
