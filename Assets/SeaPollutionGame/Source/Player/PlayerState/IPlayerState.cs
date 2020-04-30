using System;
using System.Collections.Generic;
using UnityEngine.Events;


public interface IPolluterOwner
{
    Polluter[] GetPolluters();
    void AddPolluter(Polluter polluter);
    void RemovePolluter(Polluter polluter);
    bool HasPolluter(Polluter polluter);
}

public interface ISeaEntranceOwner
{
    SeaEntrance[] GetSeaEntrances();
    void AddSeaEntrance(SeaEntrance seaEntrance);
}

public interface IEconomicState
{
    float GetMoney();
    void SetMoney(float val);
    void AddMoney(float delta);
    float GetAssetValue();
    float GetTurnIncome();
    void AccumulateMoney();
}

public interface IGoalState
{
    void SetGoals(Goal[] goals);
    Goal[] GetGoals();
    void AddToAchievedGoals(Goal goal);
    bool HasMetGoal(Goal goal);
    float GetProgress(Goal goal);
    Goal[] GetAchievedGoals();
    float GetGoalBounusScore();
}

public interface IPollutionState
{
    PollutionMap GetTurnPollutionMap(PollutionMapType type);
    PollutionMap GetAccumulatedPollutionMap(PollutionMapType type);
    void AccumulatePollution();
}

public interface IResourceState
{
    ResourceMap GetTurnResourceMap();
    ResourceMap GetAccumulatedResourceMap();
    void AccumulateResource();
}

public interface IScoreState
{
    void SetScoreWeight(ScoreWeight weight);
    float GetEfficiency();
    float GetScore();
}

public interface IPlayerState : IPolluterOwner, ISeaEntranceOwner, IEconomicState, IGoalState, IPollutionState, IResourceState, IScoreState
{
    UnityEvent GetPollutionChangeEvent(PollutionMapType type);
    UnityEvent[] GetPollutionChangeEvents();
    UnityEvent GetResourceChangeEvent();
}
