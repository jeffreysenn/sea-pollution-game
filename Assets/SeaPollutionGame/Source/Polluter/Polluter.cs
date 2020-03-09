using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EntityType
{
    NONE,
    FACTORY,
    FILTER,
};

public class Polluter : MonoBehaviour
{
    [SerializeField]
    private EntityType entityType = EntityType.NONE;
    [SerializeField]
    private TextMesh idTextMesh = null;
    [SerializeField]
    private Color onDeathColor = Color.red;
    [SerializeField]
    private Renderer meshRenderer = null;

    private PolluterAttrib polluterAttrib = null;

    private float profit = 0;
    private PollutionMap pollutionMap = new PollutionMap { };

    private int ownerID = -1;

    public int polluterId { get; set; }

    private UnityEvent pollutionMapChangeEvent = new UnityEvent { };

    public UnityEvent GetPollutionMapChangeEvent() { return pollutionMapChangeEvent; }

    public Polluter CopyAssign(Polluter rhs)
    {
        polluterAttrib = rhs.polluterAttrib;
        ownerID = rhs.ownerID;
        return this;
    }

    public void SetOwnerID(int id) { ownerID = id; }
    public int GetOwnerID() { return ownerID; }

    protected void SetProfit(float val) { profit = val; }
    public float GetProfit() { return profit; }
    protected void SetPollutionMap(PollutionMap map)
    {
        pollutionMap = map;
        pollutionMapChangeEvent.Invoke();
    }

    public PollutionMap GetPollutionMap() { return pollutionMap; }

    public void SetAttrib(PolluterAttrib attrib)
    {
        polluterAttrib = attrib;
        profit = polluterAttrib.economicAttrib.profitPerTurn;
    }

    public PolluterAttrib GetAttrib() { return polluterAttrib; }

    public void SetIdText(string s) { idTextMesh.text = s; }
    public TextMesh GetIdTextMesh() { return idTextMesh; }

    public void Operate(PollutionMap input)
    {
        pollutionMap.Clear();
        var pollutionAttrib = GetAttrib().pollutionAttrib;
        foreach (var emission in pollutionAttrib.emissions)
        {
            var pollutantName = emission.pollutantName;
            float emissionAmount = emission.emissionPerTurn;
            if (emission.emissionPerTurn < 0)
            {
                if (input.ContainsKey(pollutantName))
                {
                    float existingPollution = input[pollutantName];
                    emissionAmount = (emissionAmount + existingPollution) > 0 ? emissionAmount : -existingPollution;
                }
                else
                {
                    emissionAmount = 0;
                }
            }
            pollutionMap.Add(pollutantName, emissionAmount);
        }
        pollutionMapChangeEvent.Invoke();
    }

    protected virtual void OnDeadth()
    {
        Mulfunction();

        meshRenderer.material.SetColor("_Color", onDeathColor);
    }

    protected void Awake()
    {
        var health = GetComponent<Health>();
        health.AddDeathEventListener(OnDeadth);
    }

    public virtual void Mulfunction() { }

    public EntityType GetEntityType() { return entityType; }
}
