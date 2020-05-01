using System.Collections.Generic;

public class PlayerStateManager : IPlayerStateManager
{
    private Dictionary<int, PlayerState> playerStates = new Dictionary<int, PlayerState> { };

    public void RegisterPlayer(int id)
    {
        playerStates.Add(id, new PlayerState { });
    }

    public PlayerState GetPlayerState(int id) { return playerStates[id]; }
}

