using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PolluterAttrib
{
    public string title = "Default Polluter";
    public EconomicAttrib economicAttrib;
    public PollutionAttrib pollutionAttrib;

    public Pollution.Type GetPollutionType() { return pollutionAttrib.pollution.type; }

    public virtual string GetDescription()
    {
        return title + ":\n" + 
               economicAttrib.GetDiscription() + pollutionAttrib.GetDiscription();
    }
}

