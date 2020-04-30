using System.Collections.Generic;

public class ResourceState : IResourceState
{
    private ResourceMap accumulatedResourceMap = new ResourceMap { { "ANY", 0 } };
    private IPolluterOwner polluterOwner = null;

    public ResourceState(IPolluterOwner polluterOwner)
    {
        this.polluterOwner = polluterOwner;
    }

    public ResourceMap GetTurnResourceMap()
    {
        ResourceMap result = new ResourceMap { };
        foreach (var polluter in polluterOwner.GetPolluters())
        {
            foreach (var pair in polluter.GetResourceMap())
            {
                if (!result.ContainsKey(pair.Key)) { result.Add(pair.Key, 0); }
                result[pair.Key] += pair.Value;
            }
        }
        return result;
    }

    public ResourceMap GetAccumulatedResourceMap()
    {
        return accumulatedResourceMap;
    }

    public void AccumulateResource()
    {
        foreach (var pair in GetTurnResourceMap())
        {
            if (!accumulatedResourceMap.ContainsKey(pair.Key)) { accumulatedResourceMap.Add(pair.Key, 0); }
            accumulatedResourceMap[pair.Key] += pair.Value;

            accumulatedResourceMap["ANY"] += pair.Value;
        }
    }
}
