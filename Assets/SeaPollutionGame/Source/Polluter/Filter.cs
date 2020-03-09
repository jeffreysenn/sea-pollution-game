using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{
    public PollutionMap ComputeFilteredPollution(PollutionMap pollutionMap)
    {
        var result = new PollutionMap { };
        var pollutionAttrib = GetAttrib().pollutionAttrib;
        foreach (var emission in pollutionAttrib.emissions)
        {
            var pollutantName = emission.pollutantName;
            if (pollutionMap.ContainsKey(pollutantName))
            {
                float targetPollution = pollutionMap[pollutantName];
                float filterAbility = -emission.emissionPerTurn;
                float filtered = targetPollution > filterAbility ? filterAbility : targetPollution;
                result[pollutantName] = -filtered;
            }
        }
        return result;
    }

    private void UpdateFilteredPollution()
    {
        var filterSpace = transform.parent.GetComponent<FilterSpace>();
        var pollutionMap = filterSpace.GetPollutionMap();
        var localPollution = ComputeFilteredPollution(pollutionMap);
        filterSpace.SetLocalPollution(localPollution);
    }


    public override void Mulfunction()
    {
        SetPollutionMap(new PollutionMap { });
    }
}