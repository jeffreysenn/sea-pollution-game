﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldStateManager : MonoBehaviour
{
    [SerializeField] int turnCount = 12;
    SortedDictionary<int, PlayerState> playerStates = new SortedDictionary<int, PlayerState> { };
    int whoseTurn = 0;
    Dictionary<int, UnityEvent> endPlayerTurnEvents = new Dictionary<int, UnityEvent> { };
    UnityEvent endPlayerTurnFinishEvent = new UnityEvent { };
    UnityEvent endTurnEvent = new UnityEvent { };
    UnityEvent endGameEvent = new UnityEvent { };

    public static WorldStateManager FindWorldStateManager()
    {
        var mgrObj = FindObjectOfType<WorldStateManager>();
        return mgrObj.GetComponent<WorldStateManager>();
    }

    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
        endPlayerTurnEvents.Add(id, new UnityEvent { });
    }

    public PlayerState GetPlayerState(int id) { return playerStates[id]; }

    public void AddEndTurnEventListener(UnityAction action) { endTurnEvent.AddListener(action); }

    public void AddEndPlayerTurnEventListener(int playerID, UnityAction action)
    {
        if(!endPlayerTurnEvents.ContainsKey(playerID)) { Debug.LogWarning("[WorldStateManager] AddEndPlayerTurnEventListener: playerID " + playerID + " not found"); return; }

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

    public void AddMoney(int id, float money) { playerStates[id].money += money; }

    public void AddPollution(int id, PollutionMapType type, PollutionMap map)
    {
        playerStates[id].AddToPollutionMap(type, map);
    }

    public PollutionMap GetPollutionMap(int id, PollutionMapType type) { return playerStates[id].GetPollutionMap(type); }
    public PollutionMap GetPollutionMapSum(PollutionMapType type)
    {
        PollutionMap sum = new PollutionMap { };
        foreach (var pair in playerStates)
        {
            sum += pair.Value.GetPollutionMap(type);
        }
        return sum;
    }

    public float GetMoney(int id) { return playerStates[id].money; }
    public float GetPollution(int id, PollutionMapType type) { return playerStates[id].GetPollution(type); }
    public float GetIncome(int id) { return playerStates[id].GetIncome(); }

    public float GetPollutionSum(int id, PollutionMapType type)
    {
        float sum = 0;
        foreach (var pair in playerStates) { sum += pair.Value.GetPollution(type); }
        return sum;
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

    void EndWholeTurn()
    {
        --turnCount;
        if (turnCount == 0)
        {
            endGameEvent.Invoke();
            return;
        }
        endTurnEvent.Invoke();
    }

    // TODO(Xiaoyue Chen): another class
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

