using System;
using System.Collections.Generic;

public class PollutionMap : Dictionary<string, float>
{
    public PollutionMap(PollutionMap other) : base(other) { }
    public PollutionMap() : base() { }

    public string GetDescription()
    {
        string description = "";
        foreach(var pair in this)
        {
            description += (pair.Key + ": " + pair.Value.ToString() + "\n");
        }
        return description;
    }

    public float GetTotalPollution()
    {
        float sum = 0;
        foreach(var pair in this)
        {
            sum += pair.Value;
        }
        return sum;
    }
}
