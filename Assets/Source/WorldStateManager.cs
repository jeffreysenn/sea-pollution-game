using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WorldStateManager : MonoBehaviour
{
    [SerializeField] int turnCount = 12;
    Dictionary<int, PlayerState> playerStates;
    UnityEvent endTurnEvent = new UnityEvent { };

    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
    }

    public void SubscribeToEndTurnEvent(UnityAction action) { endTurnEvent.AddListener(action); }

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

    public void EndTurn() { 
        --turnCount;
        endTurnEvent.Invoke();
    }
}
