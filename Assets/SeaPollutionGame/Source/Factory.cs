using UnityEngine;

public class Factory : Polluter
{
    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);

        var attrib = GetAttrib();
        var space = GetSpace();
        PollutionMap map = new PollutionMap { };
        foreach (var emission in attrib.pollutionAttrib.emissions)
        {
            map.Add(emission.pollutantName, emission.emissionPerTurn);
        }
        space.SetLocalPollution(map);
        space.OutPut();
    }

    public override void Mulfunction()
    {
        stateManager.RemoveEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
    }

    public override void Remove()
    {
        base.Remove();
        var space = GetSpace();
        space.ClearLocalPollution();
        Mulfunction();
        Destroy(gameObject);
    }
}
