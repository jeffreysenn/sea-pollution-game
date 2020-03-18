using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Events;

public class ResourceMap : Dictionary<string, float> { };

public enum PollutionMapType
{
    PRODUCED,
    FILTERED,
    NET,
}

public class PlayerState
{
    private float money = 100;
    private List<Goal> achievedGoals = new List<Goal> { };
    private Dictionary<PollutionMapType, PollutionMap> accumulatedPollutionMap = new Dictionary<PollutionMapType, PollutionMap>
    {
        {PollutionMapType.PRODUCED, new PollutionMap{} },
        {PollutionMapType.FILTERED, new PollutionMap{} },
        {PollutionMapType.NET, new PollutionMap{} },
    };

    private ResourceMap accumulatedResourceMap = new ResourceMap { };

    private Dictionary<PollutionMapType, UnityEvent> stateChangeEventMap = new Dictionary<PollutionMapType, UnityEvent> {
        { PollutionMapType.PRODUCED, new UnityEvent{ } },
        { PollutionMapType.FILTERED, new UnityEvent{ } },
        { PollutionMapType.NET, new UnityEvent{ } }
    };

    private UnityEvent resourceChangeEvent = new UnityEvent { };

    private List<Polluter> polluters = new List<Polluter> { };
    private List<SeaEntrance> seaEntrances = new List<SeaEntrance> { };

    public UnityEvent GetStateChangeEvent(PollutionMapType type) { return stateChangeEventMap[type]; }
    public UnityEvent[] GetStateChangeEvents() { return stateChangeEventMap.Values.ToArray(); }
    public UnityEvent GetResourceChangeEvent() { return resourceChangeEvent; }
    public List<Goal> GetAchievedGoals() { return achievedGoals; }
    public float GetMoney() { return money; }
    public void SetMoney(float val) { money = val; }
    public void AddMoney(float delta) { SetMoney(money + delta); }

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

    public ResourceMap GetTurnResourceMap()
    {
        ResourceMap result = new ResourceMap { };
        foreach (var polluter in polluters)
        {
            foreach (var pair in polluter.GetResourceMap())
            {
                if (!result.ContainsKey(pair.Key)) { result.Add(pair.Key, 0); }
                result[pair.Key] += pair.Value;
            }
        }
        return result;
    }

    public ResourceMap GetAccumulatedResourceMap()
    {
        return accumulatedResourceMap;
    }

    public float GetGoalBounusScore()
    {
        float sum = 0;
        foreach(var goal in achievedGoals)
        {
            sum += goal.reward;
        }
        return sum;
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

    public void AccumulateMoney()
    {
        AddMoney(GetTurnIncome());
    }

    public void AccumulatePollution()
    {
        foreach (var key in accumulatedPollutionMap.Keys.ToArray())
        {
            accumulatedPollutionMap[key] += GetTurnPollutionMap(key);
            stateChangeEventMap[key].Invoke();
        }
    }

    public void AccumulateResource()
    {
        foreach (var pair in GetTurnResourceMap())
        {
            if (!accumulatedResourceMap.ContainsKey(pair.Key)) { accumulatedResourceMap.Add(pair.Key, 0); }
            accumulatedResourceMap[pair.Key] += pair.Value;
        }
        resourceChangeEvent.Invoke();
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