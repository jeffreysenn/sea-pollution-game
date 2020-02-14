using UnityEngine;

public class Factory : Polluter
{
    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
        var factorySpace = transform.parent.GetComponent<FactorySpace>();
        var pollutionSum = factorySpace.GetPollutionSum();
        var attrib = GetAttrib();
        foreach (var emission in attrib.pollutionAttrib.emissions)
        {
            pollutionSum.AddPollution(emission.pollutantName, emission.emissionPerTurn);
        }
    }
}
