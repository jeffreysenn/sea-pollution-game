using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{
    public void Operate(Dictionary<Pollution.Type, float> pollutionMap)
    {
        MakeMoney();
        var type = GetAttrib().GetPollutionType();
        float targetPollution = pollutionMap[type];
        float filterAbility = -GetAttrib().pollutionAttrib.emissionPerTurn;
        float filtered = targetPollution > filterAbility ? filterAbility : targetPollution;
        pollutionMap[type] -= filtered;
        stateManager.AddPollution(GetOwnerID(), -filtered);

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
}