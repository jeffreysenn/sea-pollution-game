using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{
    public void UpdatePollution(ref PollutionMap pollutionMap)
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
}