using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PolluterAttrib : System.ICloneable
{
    public string title = "Default Polluter";
    public EconomicAttrib economicAttrib = new EconomicAttrib { };
    public PollutionAttrib pollutionAttrib = new PollutionAttrib { };
    public VulnerabilityAttrib vulnerabilityAttrib = new VulnerabilityAttrib { };

    public object Clone()
    {
        var clone = new PolluterAttrib { };
        clone.title = (string)title.Clone();
        clone.economicAttrib = (EconomicAttrib)economicAttrib.Clone();
        clone.pollutionAttrib = (PollutionAttrib)pollutionAttrib.Clone();
        clone.vulnerabilityAttrib = (VulnerabilityAttrib)vulnerabilityAttrib.Clone();
        return clone;
    }

    public virtual string GetDescription()
    {
        return title + ":\n" + 
               economicAttrib.GetDiscription() + pollutionAttrib.GetDiscription() + vulnerabilityAttrib.GetDescription();
    }
}

