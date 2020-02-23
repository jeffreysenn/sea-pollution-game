using System.Collections;
using System.Collections.Generic;

public class PlayerState
{
    public float money = 100;
    public float producedPollution = 0;
    public float netPollution = 0;

    PollutionMap producedPollutionMap = new PollutionMap { };
    PollutionMap netPollutionMap = new PollutionMap { };

    public PollutionMap GetProducedPollutionMap() { return producedPollutionMap; }
    public PollutionMap GetNetPollutionMap() { return netPollutionMap; }
    public PollutionMap GetFilteredPollutionMap()
    {
        PollutionMap filteredPollutionMap = new PollutionMap(producedPollutionMap);
        foreach (var pair in netPollutionMap)
        {
            filteredPollutionMap[pair.Key] -= pair.Value;
        }
        return filteredPollutionMap;
    }

    public float GetNetPollution() { return netPollution; }
    public float GetFilteredPollution() { return producedPollution - netPollution; }
    public float GetProducedPollution() { return producedPollution; }
}