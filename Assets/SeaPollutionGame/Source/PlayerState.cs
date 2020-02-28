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

    PollutionMap producedPollutionMap = new PollutionMap { };
    PollutionMap filteredPollutionMap = new PollutionMap { };
    PollutionMap netPollutionMap = new PollutionMap { };

    public void SetPollutionMap(PollutionMapType type, PollutionMap map)
    {
        var pollutionMap = GetPollutionMap(type);
        pollutionMap.CopyAssign(map);
    }

    public void AddToPollutionMap(PollutionMapType type, PollutionMap map)
    {
        var pollutionMap = GetPollutionMap(type);
        pollutionMap.PlusEquals(map);
    }

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
    PollutionMap GetFilteredPollutionMap() { return filteredPollutionMap; }

    public float GetPollution(PollutionMapType type) { return Util.SumMap(GetPollutionMap(type)); }
}