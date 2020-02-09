using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : MonoBehaviour
{
    Dictionary<Pollution.Type, float> pollutionMap = new Dictionary<Pollution.Type, float> { };

    int addPollutionCallCount = 0;

    public void AddPollution(Pollution.Type type, float val) { 
        pollutionMap[type] += val;

        int childCount = transform.childCount;
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
    public float GetPollution(Pollution.Type type) { return pollutionMap[type]; }
}
