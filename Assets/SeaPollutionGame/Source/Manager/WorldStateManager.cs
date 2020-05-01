using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public interface IPlayerIDManager
{
    void RegisterPlayer(int id);
    int[] GetPlayerIDs();
}

public interface ITurnManager
{
    void SetTotalTurnCount(int count);
    void RegisterPlayer(int id);
    UnityEvent GetEndTurnEvent();
    UnityEvent GetEndPlayerTurnEvent(int playerID);
    UnityEvent GetEndPlayerTurnFinishEvent();
    UnityEvent GetEndGameEvent();
    int GetRemainingTurnCount();
    int GetCurrentPlayerID();
    void EndPlayerTurn();
}

public interface IPlayerStateManager
{
    void RegisterPlayer(int id);
    PlayerState GetPlayerState(int id);
}

public class PlayerStateManager : IPlayerStateManager
{
    private Dictionary<int, PlayerState> playerStates = new Dictionary<int, PlayerState> { };

    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
    }

    public PlayerState GetPlayerState(int id) { return playerStates[id]; }
}


public class WorldStateManager : MonoBehaviour, IPlayerIDManager, ITurnManager, IPlayerStateManager
{

    [SerializeField] private int turnCount = 18;
    IPlayerIDManager playerIDManager;
    ITurnManager turnManager;
    IPlayerStateManager playerStateManager;

    public WorldStateManager()
    {
        playerIDManager = new PlayerIDManager();
        turnManager = new TurnManager(playerIDManager);
        playerStateManager = new PlayerStateManager();
    }

    private void Start()
    {
        foreach (var id in playerIDManager.GetPlayerIDs())
        {
            var playerState = playerStateManager.GetPlayerState(id);

            foreach (var seaEntrance in FindObjectsOfType<SeaEntrance>())
            {
                if (seaEntrance.ownerID == id)
                {
                    playerState.AddSeaEntrance(seaEntrance);
                }
            }

            var endTurnEvent = turnManager.GetEndPlayerTurnEvent(id);
            endTurnEvent.AddListener(() =>
            {
                playerState.AccumulateMoney();
                playerState.AccumulatePollution();
                playerState.AccumulateResource();
            });

            var resourceChangeEvent = playerState.GetResourceChangeEvent();
            resourceChangeEvent.AddListener(() =>
            {
                foreach (var goal in playerState.GetGoals())
                {
                    if (playerState.HasMetGoal(goal) && !playerState.GetAchievedGoals().Contains(goal))
                    {
                        playerState.AddToAchievedGoals(goal);
                    }
                }
            });
        }
    }

    public int[] GetPlayerIDs()
    {
        return playerIDManager.GetPlayerIDs();
    }

    public void RegisterPlayer(int id)
    {
        playerIDManager.RegisterPlayer(id);
        turnManager.RegisterPlayer(id);
        playerStateManager.RegisterPlayer(id);
    }

    public void SetTotalTurnCount(int count)
    {
        turnManager.SetTotalTurnCount(count);
    }

    public UnityEvent GetEndTurnEvent()
    {
        return turnManager.GetEndTurnEvent();
    }

    public UnityEvent GetEndPlayerTurnEvent(int playerID)
    {
        return turnManager.GetEndPlayerTurnEvent(playerID);
    }

    public UnityEvent GetEndPlayerTurnFinishEvent()
    {
        return turnManager.GetEndPlayerTurnFinishEvent();
    }

    public UnityEvent GetEndGameEvent()
    {
        return turnManager.GetEndGameEvent();
    }

    public int GetRemainingTurnCount()
    {
        return turnManager.GetRemainingTurnCount();
    }

    public int GetCurrentPlayerID()
    {
        return turnManager.GetCurrentPlayerID();
    }

    public void EndPlayerTurn()
    {
        turnManager.EndPlayerTurn();
    }

    public PlayerState GetPlayerState(int id)
    {
        return playerStateManager.GetPlayerState(id);
    }

    public PlayerState GetCurrentPlayerState()
    {
        return GetPlayerState(GetCurrentPlayerID());
    }
}

