﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WorldStateManager : MonoBehaviour
{
    [SerializeField] int turnCount = 12;
    Dictionary<int, PlayerState> playerStates = new Dictionary<int, PlayerState> { };
    int whoseTurn = 0;
    Dictionary<int, UnityEvent> endPlayerTurnEvents = new Dictionary<int, UnityEvent> { };
    UnityEvent endTurnEvent = new UnityEvent { };

    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
        endPlayerTurnEvents.Add(id, new UnityEvent { });
    }

    public void AddEndTurnEventListener(UnityAction action) { endTurnEvent.AddListener(action); }

    public void AddEndPlayerTurnEventListener(int playerID, UnityAction action)
    {
        endPlayerTurnEvents[playerID].AddListener(action);
    }

    public int GetRemainingTurnCount() { return turnCount; }

    public int GetCurrentPlayer()
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
            if(pair.Key == playerID)
            {
                return mapIndex;
            }
            ++mapIndex;
        }
        return -1;
    }

    public void AddMoney(int id, float money) { playerStates[id].money += money; }
    public void AddPollution(int id, float pollution)
    {
        var state = playerStates[id];
        if (pollution >= 0) { state.producedPollution += pollution; }
        else { state.filteredPollution -= pollution; }
    }

    public float GetMoney(int id) { return playerStates[id].money; }
    public float GetProducedPollution(int id) { return playerStates[id].producedPollution; }
    public float GetFilteredPollution(int id) { return playerStates[id].filteredPollution; }
    public float GetNetPollution(int id) { return playerStates[id].GetNetPollution(); }
    public float GetProducedPollutionSum()
    {
        float sum = 0;
        foreach (var pair in playerStates) { sum += pair.Value.producedPollution; }
        return sum;
    }

    public float GetFilteredPollutionSum()
    {
        float sum = 0;
        foreach (var pair in playerStates) { sum += pair.Value.filteredPollution; }
        return sum;
    }

    public float GetNetPollutionSum() { return GetProducedPollutionSum() - GetFilteredPollutionSum(); }

    public void EndPlayerTurn()
    {
        int currentPlayer = GetCurrentPlayer();
        endPlayerTurnEvents[currentPlayer].Invoke();
        Debug.Log("Current player: " + currentPlayer);
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
        endTurnEvent.Invoke();
        Debug.Log("Remaining turn: " + turnCount);
    }
}
