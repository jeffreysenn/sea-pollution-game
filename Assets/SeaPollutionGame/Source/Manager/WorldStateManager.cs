using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldStateManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreWeight
    {
        public float money = 1;
        public float emission = -1;
        public float filtered = 1;
        public float efficiency = 2;
    };

    [SerializeField] int turnCount = 12;

    SortedDictionary<int, PlayerState> playerStates = new SortedDictionary<int, PlayerState> { };
    [SerializeField] ScoreWeight scoreWeight = new ScoreWeight { };
    int whoseTurn = 0;
    Dictionary<int, UnityEvent> endPlayerTurnEvents = new Dictionary<int, UnityEvent> { };

    UnityEvent endPlayerTurnFinishEvent = new UnityEvent { };
    UnityEvent endTurnEvent = new UnityEvent { };
    UnityEvent endGameEvent = new UnityEvent { };


    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
        endPlayerTurnEvents.Add(id, new UnityEvent { });
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
        if(emission == 0) { return 0; }
        return playerState.GetMoney() / emission;
    }

    public float GetScore(int playerID)
    {
        var playerState = GetPlayerState(playerID);
        return scoreWeight.money * playerState.GetMoney()
            + scoreWeight.emission * Util.SumMap(playerState.GetAccumulatedPollutionMap(PollutionMapType.NET))
            + scoreWeight.filtered * (-Util.SumMap(playerState.GetAccumulatedPollutionMap(PollutionMapType.FILTERED)))
            + scoreWeight.efficiency * GetEfficiency(playerID);
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
            });
        }
    }
}

