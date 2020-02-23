using UnityEngine;

public class Factory : Polluter
{
    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);

        var attrib = GetAttrib();
        var pollutionSum = GetPollutionSum();
        foreach (var emission in attrib.pollutionAttrib.emissions)
        {
            pollutionSum.AddPollution(emission.pollutantName, emission.emissionPerTurn);
        }
    }

    public override void Mulfunction()
    {
        stateManager.RemoveEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
    }

    public override void Remove()
    {
        base.Remove();
        var attrib = GetAttrib();
        var pollutionSum = GetPollutionSum();
        foreach (var emission in attrib.pollutionAttrib.emissions)
        {
            pollutionSum.AddPollution(emission.pollutantName, -emission.emissionPerTurn);
        }
        Mulfunction();
        Destroy(gameObject);
    }

    PollutionSum GetPollutionSum()
    {
        var factorySpace = transform.parent.GetComponent<FactorySpace>();
        return factorySpace.GetPollutionSum();
    }
}
