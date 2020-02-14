using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : MonoBehaviour
{
    Dictionary<string, float> pollutionMap = new Dictionary<string, float> { };

    int addPollutionCallCount = 0;

    public void AddPollution(string pollutantName, float val) {
        if (!pollutionMap.ContainsKey(pollutantName)) { pollutionMap.Add(pollutantName, 0); }

        pollutionMap[pollutantName] += val;

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
    public float GetPollution(string pollutantName) { return pollutionMap[pollutantName]; }
}
