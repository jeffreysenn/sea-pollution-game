using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : ITurnManager
{
    private int turnCount = 0;
    private IPlayerIDManager playerIDManager;
    private int whoseTurn = 0;
    private Dictionary<int, UnityEvent> endPlayerTurnEvents = new Dictionary<int, UnityEvent> { };
    private UnityEvent endPlayerTurnFinishEvent = new UnityEvent { };
    private UnityEvent endTurnEvent = new UnityEvent { };
    private UnityEvent endGameEvent = new UnityEvent { };

    public TurnManager(IPlayerIDManager playerIDManager) { this.playerIDManager = playerIDManager; }

    public void SetTotalTurnCount(int count) { turnCount = count; }

    public void RegisterPlayer(int id)
    {
        endPlayerTurnEvents.Add(id, new UnityEvent { });
    }


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
        var playerIDs = playerIDManager.GetPlayerIDs();
        for (int i = 0; i != playerIDs.Length; ++i)
        {
            if (i == whoseTurn) return playerIDs[i];
        }
        return -1;
    }

    public void EndPlayerTurn()
    {
        int currentPlayer = GetCurrentPlayerID();
        endPlayerTurnEvents[currentPlayer].Invoke();
        endPlayerTurnFinishEvent.Invoke();
        int index = GetPlayerIDListIndex(currentPlayer);
        var playerIDs = playerIDManager.GetPlayerIDs();
        if (index == playerIDs.Length - 1)
        {
            EndWholeTurn();
        }
        ++whoseTurn;
        whoseTurn %= playerIDs.Length;
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

    private int GetPlayerIDListIndex(int playerID)
    {
        var playerIDs = playerIDManager.GetPlayerIDs();
        for (int i = 0; i != playerIDs.Length; ++i)
        {
            if (playerIDs[i] == playerID) return i;
        }
        return -1;
    }

    public UnityEvent GetEndTurnEvent()
    {
        return endTurnEvent;
    }

    public UnityEvent GetEndPlayerTurnEvent(int playerID)
    {
        return endPlayerTurnEvents[playerID];
    }

    public UnityEvent GetEndPlayerTurnFinishEvent()
    {
        return endPlayerTurnFinishEvent;
    }

    public UnityEvent GetEndGameEvent()
    {
        return endGameEvent;
    }
}

