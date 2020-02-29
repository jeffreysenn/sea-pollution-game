using UnityEngine;

public class Factory : Polluter
{
    public override void Activate()
    {
        base.Activate();

        var attrib = GetAttrib();
        var space = GetSpace();
        PollutionMap map = new PollutionMap(attrib.pollutionAttrib.emissions);
        space.SetLocalPollution(map);
    }

    public override void Mulfunction()
    {
        base.Mulfunction();
        stateManager.RemoveEndPlayerTurnEventListener(GetOwnerID(), MakeMoney);
        var attrib = GetAttrib();
        attrib.economicAttrib.profitPerTurn = 0;
    }
}
