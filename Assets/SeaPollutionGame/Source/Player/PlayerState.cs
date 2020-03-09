using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Events;


public enum PollutionMapType
{
    PRODUCED,
    FILTERED,
    NET,
}

public class PlayerState
{
    private float money = 100;
    private Dictionary<PollutionMapType, PollutionMap> accumulatedPollutionMap = new Dictionary<PollutionMapType, PollutionMap>
    {
        {PollutionMapType.PRODUCED, new PollutionMap{} },
        {PollutionMapType.FILTERED, new PollutionMap{} },
        {PollutionMapType.NET, new PollutionMap{} },
    };

    private Dictionary<PollutionMapType, UnityEvent> stateChangeEventMap = new Dictionary<PollutionMapType, UnityEvent> {
        { PollutionMapType.PRODUCED, new UnityEvent{ } },
        { PollutionMapType.FILTERED, new UnityEvent{ } },
        { PollutionMapType.NET, new UnityEvent{ } }
    };

    private List<Polluter> polluters = new List<Polluter> { };
    private List<SeaEntrance> seaEntrances = new List<SeaEntrance> { };

    public UnityEvent GetStateChangeEvent(PollutionMapType type) { return stateChangeEventMap[type]; }
    public UnityEvent[] GetStateChangeEvents() { return stateChangeEventMap.Values.ToArray(); }

    public float GetMoney() { return money; }
    public void SetMoney(float val) { money = val; }
    public void AddMoney(float delta) { money += delta; }

    public float GetTurnIncome()
    {
        float income = 0;
        foreach (var polluter in polluters)
        {
            income += polluter.GetProfit();
        }
        return income;
    }

    public PollutionMap GetTurnPollutionMap(PollutionMapType type)
    {
        switch (type)
        {
            case PollutionMapType.PRODUCED: return SumOwnedPolluterPollutionMapIf(val => val > 0);
            case PollutionMapType.FILTERED: return SumOwnedPolluterPollutionMapIf(val => val < 0);
            case PollutionMapType.NET: return SumSeaEntrancePollution();
        }
        return null;
    }

    public PollutionMap GetAccumulatedPollutionMap(PollutionMapType type)
    {
        return accumulatedPollutionMap[type];
    }

    public void AddPolluter(Polluter polluter)
    {
        Debug.Assert(!HasPolluter(polluter));
        polluters.Add(polluter);
        var mapChangeEvent = polluter.GetPollutionMapChangeEvent();
        mapChangeEvent.AddListener(() =>
        {
            stateChangeEventMap[PollutionMapType.PRODUCED].Invoke();
            stateChangeEventMap[PollutionMapType.FILTERED].Invoke();
        });
    }

    public void RemovePolluter(Polluter polluter)
    {
        polluters.Remove(polluter);
        stateChangeEventMap[PollutionMapType.PRODUCED].Invoke();
        stateChangeEventMap[PollutionMapType.FILTERED].Invoke();
    }

    public bool HasPolluter(Polluter polluter)
    {
        return polluters.Contains(polluter);
    }

    public void AddSeaEntrance(SeaEntrance seaEntrance)
    {
        seaEntrances.Add(seaEntrance);
        foreach (var pollutionEvent in seaEntrance.GetAllPollutionEvents())
        {
            pollutionEvent.AddListener((Flow, PollutionMap) => { stateChangeEventMap[PollutionMapType.NET].Invoke(); });
        }
    }

    public void AccumulateMoney() { AddMoney(GetTurnIncome()); }

    public void AccumulatePollution()
    {
        foreach (var key in accumulatedPollutionMap.Keys.ToArray())
        {
            accumulatedPollutionMap[key] += GetTurnPollutionMap(key);
            stateChangeEventMap[key].Invoke();
        }
    }

    private PollutionMap SumOwnedPolluterPollutionMapIf(System.Predicate<float> pred)
    {
        var result = new PollutionMap { };
        foreach (var polluter in polluters)
        {
            foreach (var pair in polluter.GetPollutionMap())
            {
                if (!result.ContainsKey(pair.Key)) { result.Add(pair.Key, 0); }
                if (pred(pair.Value)) { result[pair.Key] += pair.Value; }
            }
        }
        return result;
    }

    private PollutionMap SumSeaEntrancePollution()
    {
        var result = new PollutionMap { };
        foreach (var seaEntrance in seaEntrances)
        {
            result += seaEntrance.GetPollutionMap();
        }
        return result;
    }

}