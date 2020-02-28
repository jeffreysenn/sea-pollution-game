using System;
using System.Collections.Generic;

[System.Serializable]
public class PollutionMap : Dictionary<string, float>
{
    public PollutionMap(PollutionMap other) : base(other) { }
    public PollutionMap() : base() { }
    public PollutionMap(PollutionAttrib.Emission[] emissions) : base()
    {
        foreach (var emission in emissions)
        {
            Add(emission.pollutantName, emission.emissionPerTurn);
        }
    }

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

    public PollutionMap CopyAssign(PollutionMap rhs)
    {
        Clear();
        foreach(var pair in rhs)
        {
            Add(pair.Key, pair.Value);
        }
        return this;
    }

    public PollutionMap PlusEquals(PollutionMap rhs)
    {
        var result = this + rhs;
        CopyAssign(result);
        return this;
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
