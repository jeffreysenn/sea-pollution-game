using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{
    public void Operate(Dictionary<string, float> pollutionMap)
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
                stateManager.AddPollution(GetOwnerID(), -filtered);
            }
        }

        var parentFilterSpaceObj = transform.parent.parent.gameObject;
        if (parentFilterSpaceObj)
        {
            var parentFilterSpace = parentFilterSpaceObj.GetComponent<FilterSpace>();
            if (parentFilterSpace && parentFilterSpace.polluter)
            {
                var parentFilter = (Filter)parentFilterSpace.polluter;
                parentFilter.Operate(pollutionMap);
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
    }
}