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
    RECYCLER
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
    [SerializeField]
    private Health health = null;

    private PolluterAttrib polluterAttrib = null;

    private float profit = 0;
    private PollutionMap pollutionMap = new PollutionMap { };
    private ResourceMap resourceMap = new ResourceMap { };

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

    protected void SetResourceMap(ResourceMap map) { resourceMap = map; }
    public ResourceMap GetResourceMap() { return resourceMap; }

    public void SetAttrib(PolluterAttrib attrib)
    {
        polluterAttrib = attrib;
        profit = polluterAttrib.economicAttrib.profitPerTurn;
    }

    public PolluterAttrib GetAttrib() { return polluterAttrib; }

    public void SetIdText(string s) { idTextMesh.text = s; }
    public TextMesh GetIdTextMesh() { return idTextMesh; }
    public Health GetHealthComp() { return health; }
    public virtual void Operate(PollutionMap input)
    {
        if (!GetHealthComp().IsAlive()) { return; }

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


        resourceMap.Clear();
        var resourceAttrib = GetAttrib().resourceAttrib;
        if (resourceAttrib.products != null)
        {
            foreach (var product in resourceAttrib.products)
            {
                resourceMap.Add(product.resourceName, product.productPerTurn);
            }
        }
    }

    protected virtual void OnDeadth()
    {
        Mulfunction();

        meshRenderer.material.SetColor("_Color", onDeathColor);
    }

    protected void Awake()
    {
        health.AddDeathEventListener(OnDeadth);
    }

    public virtual void Mulfunction() {
        resourceMap.Clear();
    }

    public EntityType GetEntityType() { return entityType; }

    public bool IsAlive() { return health.IsAlive(); }
}
