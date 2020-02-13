using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : MonoBehaviour
{
    Dictionary<Pollutant.Type, float> pollutionMap = new Dictionary<Pollutant.Type, float> { };

    int addPollutionCallCount = 0;

    public void AddPollution(Pollutant.Type type, float val) {
        if (!pollutionMap.ContainsKey(type)) { pollutionMap.Add(type, 0); }

        pollutionMap[type] += val;

        FactorySpace[] factorySpaces = GetComponentsInChildren<FactorySpace>();
        int childCount = 0;
        foreach(var factorySpace in factorySpaces)
        {
            childCount += Convert.ToInt32(factorySpace.polluter != null);
        }
        if(++addPollutionCallCount == childCount)
        {
            addPollutionCallCount = 0;
            var filterSpace = transform.parent.GetComponent<FilterSpace>();
            var polluter = filterSpace.polluter;
            if (polluter)
            {
                var filter = (Filter)polluter;
                filter.Operate(pollutionMap);
            }
        }

    }
    public float GetPollution(Pollutant.Type type) { return pollutionMap[type]; }
}
