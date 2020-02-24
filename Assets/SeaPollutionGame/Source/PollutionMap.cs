using System;
using System.Collections.Generic;

public class PollutionMap : Dictionary<string, float>
{
    public PollutionMap(PollutionMap other) : base(other) { }
    public PollutionMap() : base() { }

    public static PollutionMap operator +(PollutionMap lhs, PollutionMap rhs)
    {
        var result = new PollutionMap(lhs);
        foreach (var pair in rhs)
        {
            if (!result.ContainsKey(pair.Key)) { result.Add(pair.Key, pair.Value); }
            else { result[pair.Key] += pair.Value; }
        }
        return result;
    }

    public string GetDescription()
    {
        string description = "";
        foreach (var pair in this)
        {
            description += (pair.Key + ": " + pair.Value.ToString() + "\n");
        }
        return description;
    }

    public float GetTotalPollution()
    {
        float sum = 0;
        foreach (var pair in this)
        {
            sum += pair.Value;
        }
        return sum;
    }
}
