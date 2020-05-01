using System.Collections.Generic;

public class StateHistoryManager : IStateHistoryManager
{
    private Dictionary<int, StateHistory> histories = new Dictionary<int, StateHistory> { };

    public void RegisterPlayer(int id)
    {
        histories.Add(id, new StateHistory { });
    }

    public StateHistory GetStateHistory(int id)
    {
        return histories[id];
    }
}

