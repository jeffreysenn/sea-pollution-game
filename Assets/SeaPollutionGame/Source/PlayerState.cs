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
    List<Polluter> ownedPolluters = new List<Polluter> { };

    public float GetIncome() {
        float income = 0;
        foreach(var polluter in ownedPolluters)
        {
            income += polluter.GetAttrib().economicAttrib.profitPerTurn;
        }
        return income;
    }

    public void AddPolluter(Polluter polluter)
    {
        Debug.Assert(!HasPolluter(polluter));
        ownedPolluters.Add(polluter);
    }

    public void RemovePolluter(Polluter polluter)
    {
        ownedPolluters.Remove(polluter);
    }

    public bool HasPolluter(Polluter polluter)
    {
        return ownedPolluters.Contains(polluter);
    }

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