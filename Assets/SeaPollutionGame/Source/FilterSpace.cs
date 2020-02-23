using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterSpace : Space
{
    PollutionMap pollutionMap = new PollutionMap { };
    PollutionMap filteredPollutionMap = new PollutionMap { };

    public void UpdatePollution(PollutionMap map)
    {
        pollutionMap = new PollutionMap(map);
        UseFilter();
    }

    public void UseFilter()
    {
        filteredPollutionMap.Clear();

        if (polluter)
        {
            var filter = (Filter)polluter;
            filter.FilterPollution(ref pollutionMap, ref filteredPollutionMap);
        }
        ForwardPollutionMap();
    }

    void ForwardPollutionMap()
    {
        var parentFilterSpace = transform.parent.GetComponent<FilterSpace>();
        if (parentFilterSpace)
        {
            parentFilterSpace.UpdatePollution(pollutionMap);
        }
        else
        {
            var parentSeaEntrance = transform.parent.GetComponent<SeaEntrance>();
            parentSeaEntrance.SetPollutionMap(pollutionMap);
        }
    }

    public void RemoveFilter()
    {
        polluter = null;
        foreach(var pair in filteredPollutionMap)
        {
            pollutionMap[pair.Key] += pair.Value;
        }
        filteredPollutionMap.Clear();
        ForwardPollutionMap();
    }
}
