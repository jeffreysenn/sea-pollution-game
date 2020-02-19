using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{
    public void FilterPollution(ref PollutionMap pollutionMap, ref PollutionMap filteredPollutionMap)
    {
        var pollutionAttrib = GetAttrib().pollutionAttrib;
        foreach(var emission in pollutionAttrib.emissions)
        {
            var pollutantName = emission.pollutantName;
            if (pollutionMap.ContainsKey(pollutantName))
            {
                float targetPollution = pollutionMap[pollutantName];
                float filterAbility = -emission.emissionPerTurn;
                float filtered = targetPollution > filterAbility ? filterAbility : targetPollution;
                if (!filteredPollutionMap.ContainsKey(pollutantName)) { filteredPollutionMap.Add(pollutantName, 0); }
                filteredPollutionMap[pollutantName] += filtered;
                pollutionMap[pollutantName] -= filtered;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
        var filterSpace = transform.parent.GetComponent<FilterSpace>();
        filterSpace.UseFilter();
    }

    public override void Mulfunction()
    {
        var filterSpace = transform.parent.GetComponent<FilterSpace>();
        filterSpace.RemoveFilter();
    }

    public override void Remove()
    {
        base.Remove();
        Mulfunction();
        Destroy(gameObject);
    }
}