using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluterAttrib : ScriptableObject
{
    public string polluterName = "Default Polluter";
    public EconomicAttrib economicAttrib = new EconomicAttrib { costToPurchase = 40.0f, profitPerTurn = 10.0f };
    public PollutionAttrib pollutionAttrib = new PollutionAttrib { pollution = null, emissionPerTurn = 1.0f };

    public Pollution.Type GetPollutionType() { return pollutionAttrib.pollution.type; }

    public virtual string GetDescription()
    {
        return polluterName + ":\n" + 
               economicAttrib.GetDiscription() + pollutionAttrib.GetDiscription();
    }
}

