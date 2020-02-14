using System;
using System.Collections.Generic;

public class PollutionMap : Dictionary<string, float>
{
    public string GetDescription()
    {
        string description = "";
        foreach(var pair in this)
        {
            description += (pair.Key + ": " + pair.Value.ToString() + "\n");
        }
        return description;
    }
}
