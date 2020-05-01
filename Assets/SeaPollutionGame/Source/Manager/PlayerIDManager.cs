using System.Collections.Generic;

public class PlayerIDManager : IPlayerIDManager
{
    private SortedSet<int> playerIDs = new SortedSet<int> { };

    public int[] GetPlayerIDs()
    {
        var result = new int[playerIDs.Count];
        playerIDs.CopyTo(result);
        return result;
    }

    public void RegisterPlayer(int id)
    {
        playerIDs.Add(id);
    }
}

