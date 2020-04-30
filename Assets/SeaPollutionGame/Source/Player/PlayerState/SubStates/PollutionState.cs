using System;
using System.Collections.Generic;
using System.Linq;

public class PollutionState : IPollutionState
{
    private Dictionary<PollutionMapType, PollutionMap> accumulatedPollutionMap = new Dictionary<PollutionMapType, PollutionMap> { };

    private IPolluterOwner polluterOwner = null;
    private ISeaEntranceOwner seaEntranceOwner = null;

    public PollutionState(IPolluterOwner polluters, ISeaEntranceOwner seaEntrances)
    {
        foreach (PollutionMapType type in Enum.GetValues(typeof(PollutionMapType)))
        {
            accumulatedPollutionMap.Add(type, new PollutionMap());
        }
        this.polluterOwner = polluters;
        this.seaEntranceOwner = seaEntrances;
    }

    public PollutionMap GetTurnPollutionMap(PollutionMapType type)
    {
        switch (type)
        {
            case PollutionMapType.PRODUCED: return SumOwnedPolluterPollutionMapIf(polluter => polluter.GetComponent<Factory>());
            case PollutionMapType.FILTERED: return SumOwnedPolluterPollutionMapIf(polluter => polluter.GetComponent<Filter>());
            case PollutionMapType.RECYCLED: return SumOwnedPolluterPollutionMapIf(polluter => polluter.GetComponent<Recycler>());
            case PollutionMapType.NET: return SumSeaEntrancePollution();
        }
        return null;
    }

    public PollutionMap GetAccumulatedPollutionMap(PollutionMapType type)
    {
        return accumulatedPollutionMap[type];
    }

    public void AccumulatePollution()
    {
        foreach (var key in accumulatedPollutionMap.Keys.ToArray())
        {
            accumulatedPollutionMap[key] += GetTurnPollutionMap(key);
        }
    }

    private PollutionMap SumOwnedPolluterPollutionMapIf(System.Predicate<Polluter> polluterPred)
    {
        var result = new PollutionMap { };
        foreach (var polluter in polluterOwner.GetPolluters())
        {
            if (polluterPred(polluter))
            {
                foreach (var pair in polluter.GetPollutionMap())
                {
                    if (!result.ContainsKey(pair.Key)) { result.Add(pair.Key, 0); }
                    result[pair.Key] += pair.Value;
                }
            }
        }
        return result;
    }

    private PollutionMap SumSeaEntrancePollution()
    {
        var result = new PollutionMap { };
        foreach (var seaEntrance in seaEntranceOwner.GetSeaEntrances())
        {
            result += seaEntrance.GetPollutionMap();
        }
        return result;
    }
}
