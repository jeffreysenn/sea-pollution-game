using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class ResourceMap : Dictionary<string, float> { };

public enum PollutionMapType
{
    PRODUCED,
    FILTERED,
    NET,
    RECYCLED,
}

public class PlayerState : IPlayerState
{
    private IPolluterOwner polluterOwner;
    private ISeaEntranceOwner seaEntranceOwner;
    private IEconomicState economicState;
    private IPollutionState pollutionState;
    private IResourceState resourceState;
    private IGoalState goalState;
    private IScoreState scoreState;
    private Dictionary<PollutionMapType, UnityEvent> stateChangeEventMap = new Dictionary<PollutionMapType, UnityEvent> {
        { PollutionMapType.PRODUCED, new UnityEvent{ } },
        { PollutionMapType.FILTERED, new UnityEvent{ } },
        { PollutionMapType.RECYCLED, new UnityEvent{ } },
        { PollutionMapType.NET, new UnityEvent{ } }
    };
    private UnityEvent resourceChangeEvent = new UnityEvent { };

    public PlayerState()
    {
        polluterOwner = new PolluterOwner();
        seaEntranceOwner = new SeaEntranceOwner();
        economicState = new EconomicState(120, polluterOwner);
        pollutionState = new PollutionState(polluterOwner, seaEntranceOwner);
        resourceState = new ResourceState(polluterOwner);
        goalState = new GoalState(resourceState);
        scoreState = new ScoreState(pollutionState, economicState, goalState);
    }

    public void AccumulateMoney()
    {
        economicState.AccumulateMoney();
    }

    public void AccumulatePollution()
    {
        pollutionState.AccumulatePollution();
        foreach (var e in GetPollutionChangeEvents()) e.Invoke();
    }

    public void AccumulateResource()
    {
        resourceState.AccumulateResource();
        resourceChangeEvent.Invoke();
    }

    public void AddMoney(float delta)
    {
        economicState.AddMoney(delta);
    }

    public void AddPolluter(Polluter polluter)
    {
        polluterOwner.AddPolluter(polluter);
        var mapChangeEvent = polluter.GetPollutionMapChangeEvent();
        mapChangeEvent.AddListener(() =>
        {
            stateChangeEventMap[PollutionMapType.PRODUCED].Invoke();
            stateChangeEventMap[PollutionMapType.FILTERED].Invoke();
            stateChangeEventMap[PollutionMapType.RECYCLED].Invoke();
        });
    }

    public void AddSeaEntrance(SeaEntrance seaEntrance)
    {
        seaEntranceOwner.AddSeaEntrance(seaEntrance);
        foreach (var pollutionEvent in seaEntrance.GetAllPollutionEvents())
        {
            pollutionEvent.AddListener((Flow, PollutionMap) => { stateChangeEventMap[PollutionMapType.NET].Invoke(); });
        }
    }

    public void AddToAchievedGoals(Goal goal)
    {
        goalState.AddToAchievedGoals(goal);
    }

    public PollutionMap GetAccumulatedPollutionMap(PollutionMapType type)
    {
        return pollutionState.GetAccumulatedPollutionMap(type);
    }

    public ResourceMap GetAccumulatedResourceMap()
    {
        return resourceState.GetAccumulatedResourceMap();
    }

    public Goal[] GetAchievedGoals()
    {
        return goalState.GetAchievedGoals();
    }

    public float GetAssetValue()
    {
        return economicState.GetAssetValue();
    }

    public float GetEfficiency()
    {
        return scoreState.GetEfficiency();
    }

    public float GetGoalBounusScore()
    {
        return goalState.GetGoalBounusScore();
    }

    public Goal[] GetGoals()
    {
        return goalState.GetGoals();
    }

    public float GetMoney()
    {
        return economicState.GetMoney();
    }

    public Polluter[] GetPolluters()
    {
        return polluterOwner.GetPolluters();
    }

    public UnityEvent GetPollutionChangeEvent(PollutionMapType type) { return stateChangeEventMap[type]; }
    public UnityEvent[] GetPollutionChangeEvents() { return stateChangeEventMap.Values.ToArray(); }

    public float GetProgress(Goal goal)
    {
        return goalState.GetProgress(goal);
    }

    public UnityEvent GetResourceChangeEvent() { return resourceChangeEvent; }

    public float GetScore()
    {
        return scoreState.GetScore();
    }

    public SeaEntrance[] GetSeaEntrances()
    {
        return seaEntranceOwner.GetSeaEntrances();
    }

    public float GetTurnIncome()
    {
        return economicState.GetTurnIncome();
    }

    public PollutionMap GetTurnPollutionMap(PollutionMapType type)
    {
        return pollutionState.GetTurnPollutionMap(type);
    }

    public ResourceMap GetTurnResourceMap()
    {
        return resourceState.GetTurnResourceMap();
    }

    public bool HasMetGoal(Goal goal)
    {
        return goalState.HasMetGoal(goal);
    }

    public bool HasPolluter(Polluter polluter)
    {
        return polluterOwner.HasPolluter(polluter);
    }

    public void RemovePolluter(Polluter polluter)
    {
        polluterOwner.RemovePolluter(polluter);
        foreach (var e in GetPollutionChangeEvents()) e.Invoke();
    }

    public void SetGoals(Goal[] goals)
    {
        goalState.SetGoals(goals);
    }

    public void SetMoney(float val)
    {
        economicState.SetMoney(val);
    }

    public void SetScoreWeight(ScoreWeight weight)
    {
        scoreState.SetScoreWeight(weight);
    }
}