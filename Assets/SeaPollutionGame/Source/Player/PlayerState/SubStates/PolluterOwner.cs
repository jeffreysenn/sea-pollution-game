using System.Collections.Generic;
using System.Diagnostics;

public class PolluterOwner : IPolluterOwner
{
    private List<Polluter> polluters = new List<Polluter> { };

    public Polluter[] GetPolluters()
    {
        return polluters.ToArray();
    }

    public void AddPolluter(Polluter polluter)
    {
        Debug.Assert(!HasPolluter(polluter));
        polluters.Add(polluter);
    }

    public void RemovePolluter(Polluter polluter)
    {
        Debug.Assert(HasPolluter(polluter));
        polluters.Remove(polluter);
    }

    public bool HasPolluter(Polluter polluter)
    {
        return polluters.Contains(polluter);
    }
}
