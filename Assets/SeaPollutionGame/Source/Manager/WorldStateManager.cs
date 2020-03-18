using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ScoreWeight
{
    public float money = 1;
    public float emission = 0.5f;
    public float filtered = 1;
    public float efficiency = 2;
};

public class WorldStateManager : MonoBehaviour
{

    [SerializeField] private int turnCount = 12;

    private SortedDictionary<int, PlayerState> playerStates = new SortedDictionary<int, PlayerState> { };
    private ScoreWeight scoreWeight = new ScoreWeight { };
    private Goal[] goals = null;

    private int whoseTurn = 0;
    private Dictionary<int, UnityEvent> endPlayerTurnEvents = new Dictionary<int, UnityEvent> { };
    private UnityEvent endPlayerTurnFinishEvent = new UnityEvent { };
    private UnityEvent endTurnEvent = new UnityEvent { };
    private UnityEvent endGameEvent = new UnityEvent { };

    public void SetScoreWeight(ScoreWeight weight) { scoreWeight = weight; }

    public void SetGoals(Goal[] goals) { this.goals = goals; }
    public Goal[] GetGoals() { return goals; }
    public bool HasPlayerMetGoal(Goal goal, int playerID)
    {
        return GetPlayerProgress(goal, playerID) == 1;
    }

    public float GetPlayerProgress(Goal goal, int playerID)
    {
        var playerState = GetPlayerState(playerID);
        var resourceMap = playerState.GetAccumulatedResourceMap();
        float val = 0;
        if (resourceMap.TryGetValue(goal.resourceName, out val))
        {
            return goal.GetProgress(val);
        }
        return 0;
    }

    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
        endPlayerTurnEvents.Add(id, new UnityEvent { });
        playerStates[id].GetResourceChangeEvent().AddListener(UpdateAchievedGoalName);
    }

    public PlayerState GetPlayerState(int id) { return playerStates[id]; }
    public PlayerState GetCurrentPlayerState() { return playerStates[GetCurrentPlayerID()]; }

    public void AddEndTurnEventListener(UnityAction action) { endTurnEvent.AddListener(action); }

    public void AddEndPlayerTurnEventListener(int playerID, UnityAction action)
    {
        if (!endPlayerTurnEvents.ContainsKey(playerID)) { Debug.LogError("[WorldStateManager] AddEndPlayerTurnEventListener: playerID " + playerID + " not found"); return; }

        endPlayerTurnEvents[playerID].AddListener(action);
    }

    public void AddEndPlayerTurnFinishEventListener(UnityAction action)
    {
        endPlayerTurnFinishEvent.AddListener(action);
    }

    public void RemoveEndPlayerTurnEventListener(int playerID, UnityAction action)
    {
        endPlayerTurnEvents[playerID].RemoveListener(action);
    }

    public void AddEndGameEventListener(UnityAction action) { endGameEvent.AddListener(action); }

    public int GetRemainingTurnCount() { return turnCount; }

    public int GetCurrentPlayerID()
    {
        int mapIndex = 0;
        foreach (var pair in playerStates)
        {
            if (mapIndex++ == whoseTurn)
            {
                return pair.Key;
            }
        }
        return -1;
    }

    int GetPlayerMapIndex(int playerID)
    {
        int mapIndex = 0;
        foreach (var pair in playerStates)
        {
            if (pair.Key == playerID)
            {
                return mapIndex;
            }
            ++mapIndex;
        }
        return -1;
    }

    public PollutionMap GetPollutionMapSum(PollutionMapType type)
    {
        PollutionMap sum = new PollutionMap { };
        foreach (var pair in playerStates)
        {
            sum += pair.Value.GetAccumulatedPollutionMap(type);
        }
        return sum;
    }

    public float GetPollutionSum(PollutionMapType type)
    {
        float sum = 0;
        foreach (var pair in playerStates) { sum += Util.SumMap(pair.Value.GetAccumulatedPollutionMap(type)); }
        return sum;
    }

    public float GetEfficiency(int playerID)
    {
        var playerState = GetPlayerState(playerID);
        float emission = Util.SumMap(playerState.GetAccumulatedPollutionMap(PollutionMapType.NET));
        if (emission == 0) { return 0; }
        return playerState.GetMoney() / emission;
    }

    public float GetScore(int playerID)
    {
        var playerState = GetPlayerState(playerID);
        float score = scoreWeight.money * (playerState.GetMoney() + playerState.GetAssetValue())
            + scoreWeight.filtered * (-Util.SumMap(playerState.GetAccumulatedPollutionMap(PollutionMapType.FILTERED)))
            + scoreWeight.efficiency * GetEfficiency(playerID)
            + playerState.GetGoalBounusScore();
        return score;
    }

    public void EndPlayerTurn()
    {
        if (turnCount == 0) return;

        int currentPlayer = GetCurrentPlayerID();
        endPlayerTurnEvents[currentPlayer].Invoke();
        endPlayerTurnFinishEvent.Invoke();
        int mapIndex = GetPlayerMapIndex(currentPlayer);
        if (mapIndex == playerStates.Count - 1)
        {
            EndWholeTurn();
        }

        ++whoseTurn;
        whoseTurn %= playerStates.Count;
    }

    private void EndWholeTurn()
    {
        --turnCount;
        if (turnCount == 0)
        {
            endGameEvent.Invoke();
            return;
        }
        endTurnEvent.Invoke();
    }

    private void Start()
    {
        foreach (var pair in playerStates)
        {
            foreach (var seaEntrance in FindObjectsOfType<SeaEntrance>())
            {
                if (seaEntrance.ownerID == pair.Key) { pair.Value.AddSeaEntrance(seaEntrance); }
            }
        }

        foreach (var pair in endPlayerTurnEvents)
        {
            pair.Value.AddListener(() =>
            {
                var playerState = GetPlayerState(pair.Key);
                playerState.AccumulateMoney();
                playerState.AccumulatePollution();
                playerState.AccumulateResource();
            });
        }
    }

    private void UpdateAchievedGoalName()
    {
        foreach (var pair in playerStates)
        {
            foreach (var goal in goals)
            {
                if (HasPlayerMetGoal(goal, pair.Key))
                {
                    var achievedGoals = pair.Value.GetAchievedGoals();
                    if (!achievedGoals.Contains(goal)) { achievedGoals.Add(goal); }
                }
            }
        }
    }
}

