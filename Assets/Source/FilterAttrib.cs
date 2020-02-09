using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FilterAttrib", menuName = "FilterAttrib")]
public class FilterAttrib : ScriptableObject
{
    public string filterName = "default filter";
    public EconomicAttrib economicAttrib = new EconomicAttrib { costToPurchase = 30, profitPerTurn = -5 };
    public PollutionAttrib pollutionAttrib = new PollutionAttrib { pollution = null, emissionPerTurn = -0.5f };

    public string GetDescription()
    {
        return economicAttrib.GetDiscription() + pollutionAttrib.GetDiscription();
    }
}
