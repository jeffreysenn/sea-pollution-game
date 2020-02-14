using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterSpace : Space
{
    PollutionMap pollutionMap = new PollutionMap { };

    public void UpdatePollution(PollutionMap map)
    {
        pollutionMap = map;
        UseFilter();
    }

    public void UseFilter()
    {
        if (polluter)
        {
            var filter = (Filter)polluter;
            filter.UpdatePollution(ref pollutionMap);
        }
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
}
