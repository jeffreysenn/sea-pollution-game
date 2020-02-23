using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public enum PollutionMapType
{
    PRODUCED,
    FILTERED,
    NET,
}

public class PlayerState
{
    public float money = 100;
    public float producedPollution = 0;
    public float netPollution = 0;

    public PollutionMap producedPollutionMap = new PollutionMap { };
    public PollutionMap netPollutionMap = new PollutionMap { };

    public PollutionMap GetPollutionMap(PollutionMapType type)
    {
        switch (type)
        {
            case PollutionMapType.PRODUCED: return GetProducedPollutionMap();
            case PollutionMapType.FILTERED: return GetFilteredPollutionMap();
            case PollutionMapType.NET: return GetNetPollutionMap();
        }
        Debug.Assert(false);
        return new PollutionMap { };
    }

    PollutionMap GetProducedPollutionMap() { return producedPollutionMap; }
    PollutionMap GetNetPollutionMap() { return netPollutionMap; }
    PollutionMap GetFilteredPollutionMap()
    {
        PollutionMap filteredPollutionMap = new PollutionMap(producedPollutionMap);
        foreach (var pair in netPollutionMap)
        {
            filteredPollutionMap[pair.Key] -= pair.Value;
        }
        return filteredPollutionMap;
    }

    public float GetNetPollution() { return netPollution; }
    public float GetFilteredPollution() { return producedPollution - netPollution; }
    public float GetProducedPollution() { return producedPollution; }
}