using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PolluterAttrib
{
    public string title = "Default Polluter";
    public EconomicAttrib economicAttrib;
    public PollutionAttrib pollutionAttrib;
    public VulnerabilityAttrib vulnerabilityAttrib;

    public virtual string GetDescription()
    {
        return title + ":\n" + 
               economicAttrib.GetDiscription() + pollutionAttrib.GetDiscription() + vulnerabilityAttrib.GetDescription();
    }
}

