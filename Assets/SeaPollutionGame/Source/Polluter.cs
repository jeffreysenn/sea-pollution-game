using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polluter : MonoBehaviour
{
    private PolluterAttrib polluterAttrib = null;
    protected WorldStateManager stateManager = null;

    private int ownerID = -1;

    public Polluter Copy(Polluter other)
    {
        polluterAttrib = other.polluterAttrib;
        stateManager = other.stateManager;
        ownerID = other.ownerID;
        return this;
    }

    public void SetOwnerID(int id) { ownerID = id; }
    public int GetOwnerID() { return ownerID; }

    public void SetAttrib(PolluterAttrib attrib) { polluterAttrib = attrib; }

    public PolluterAttrib GetAttrib() { return polluterAttrib; }

    public virtual void OnDeadth()
    {
        Mulfunction();
        var meshFilter = GetComponentInChildren<MeshFilter>();
        var renderer = meshFilter.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", Color.red);
    }

    protected void Awake()
    {
        stateManager = WorldStateManager.FindWorldStateManager();
    }

    protected void MakeMoney()
    {
        var profit = polluterAttrib.economicAttrib.profitPerTurn;
        stateManager.AddMoney(ownerID, profit);
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
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
        gameObject.AddComponent<Remove>();
        var health = gameObject.AddComponent<Health>();
        health.AddDeathEventListener(OnDeadth);
    }

    public bool IsActive() { return GetSpace(); }
    public Space GetSpace() { return transform.parent.GetComponent<Space>(); }
    public virtual void Mulfunction() { }

    public bool CanRemove()
    {
        if (GetOwnerID() == stateManager.GetCurrentPlayerID()
            && GetAttrib().economicAttrib.removalCost <= stateManager.GetMoney(GetOwnerID()))
        {
            return true;
        }
        return false;
    }

    public void Remove()
    {
        if(IsActive())
        {
            stateManager.AddMoney(GetOwnerID(), -GetAttrib().economicAttrib.removalCost);
            Mulfunction();
            Destroy(gameObject);
        }
    }

    protected virtual void OnDisable()
    {
        if(IsActive())
        {
            var space = GetSpace();
            space.ClearLocalPollution();
            space.polluter = null;
        }
    }

}
