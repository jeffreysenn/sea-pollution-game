using UnityEngine;

public class Factory : Polluter
{
    PollutionSum pollutionSum = null;

    public void Operate()
    {
        MakeMoney();
        Pollute();

        var attrib = GetAttrib();
        foreach (var emission in attrib.pollutionAttrib.emissions)
        {
            pollutionSum.AddPollution(emission.pollutantName, emission.emissionPerTurn);
        }
    }

    public override void Activate()
    {
        base.Activate();
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), Operate);
        pollutionSum = transform.parent.parent.GetComponent<PollutionSum>();
    }
}
