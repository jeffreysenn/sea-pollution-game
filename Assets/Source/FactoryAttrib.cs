using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FactoryAttrib", menuName = "FactoryAttrib")]
public class FactoryAttrib : ScriptableObject
{
    public string factoryName = "Default Factory";
    public EconomicAttrib economicAttrib = new EconomicAttrib { costToPurchase = 40.0f, profitPerTurn = 10.0f };
    public PollutionAttrib pollutionAttrib = new PollutionAttrib { pollution = null, emissionPerTurn = 1.0f };
    
    public string GetDescription()
    {
        return economicAttrib.GetDiscription() + pollutionAttrib.GetDiscription();
    }
}

