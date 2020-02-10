using UnityEngine;

public class Factory : Polluter
{
    PollutionSum pollutionSum = null;

    public void Operate()
    {
        MakeMoney();
        Pollute();

        var attrib = GetAttrib();
        var type = attrib.GetPollutionType();
        var emission = attrib.pollutionAttrib.emissionPerTurn;
        pollutionSum.AddPollution(type, emission);
    }

    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), Operate);
        pollutionSum = transform.parent.parent.GetComponent<PollutionSum>();
    }
}
