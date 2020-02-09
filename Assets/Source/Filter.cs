using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{

    [SerializeField] FilterAttrib attrib = null;

    void Awake()
    {
        polluterAttrib = attrib;
    }

    public void Operate(Dictionary<Pollution.Type, float> pollutionMap)
    {
        MakeMoney();
        var type = attrib.GetPollutionType();
        float targetPollution = pollutionMap[type];
        float filterAbility = -attrib.pollutionAttrib.emissionPerTurn;
        float filtered = targetPollution > filterAbility ? filterAbility : targetPollution;
        pollutionMap[type] -= filtered;
        stateManager.AddPollution(GetOwnerID(), filtered);
        if(transform.childCount != 0)
        {
            var nextFilter = GetComponentInChildren<Filter>();
            nextFilter.Operate(pollutionMap);
        }
    }
}