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
    [Serializable]
    class DeathEffect
    {
        public string disasterName = null;
        public GameObject effectObject = null;
    }

    [SerializeField]
    private EntityType entityType = EntityType.NONE;
    [SerializeField]
    private TextMesh idTextMesh = null;
    [Header("Death")]
    [SerializeField]
    private Health health = null;
    [SerializeField]
    private GameObject flareObject = null;
    [SerializeField]
    private Color onDeathColor = Color.red;
    [SerializeField]
    private Material onDeathMaterial = null;
    [SerializeField]
    private Renderer meshRenderer = null;
    [SerializeField]
    private Color onDeathTextColor = Color.black;
    [Header("Death Effects")]
    [SerializeField]
    private List<DeathEffect> deathEffects = new List<DeathEffect>();

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
        if (pollutionAttrib.emissions != null)
        {
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

    protected virtual void OnDeath(Disaster from)
    {
        Mulfunction();

        //meshRenderer.material.SetColor("_Color", onDeathColor);
        meshRenderer.material = onDeathMaterial;
        flareObject.SetActive(true);
        idTextMesh.color = onDeathTextColor;
        
        if(from != null)
        {
            foreach (DeathEffect de in deathEffects)
                de.effectObject.SetActive(de.disasterName == from.title);
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        if(audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }

    protected void Awake()
    {
        flareObject.SetActive(false);

        foreach (DeathEffect de in deathEffects)
            de.effectObject.SetActive(false);

        //health.AddDeathEventListener(OnDeath);
        health.OnDeathFrom += OnDeath;
    }

    public virtual void Mulfunction()
    {
        resourceMap.Clear();
    }

    public EntityType GetEntityType() { return entityType; }

    public bool IsAlive() { return health.IsAlive(); }
}
