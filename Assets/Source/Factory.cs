using UnityEngine;

public class Factory : Polluter
{
    [SerializeField] FactoryAttrib attrib = null;

    PollutionSum pollutionSum = null;

    void Awake()
    {
        polluterAttrib = attrib;
    }

    public void Operate()
    {
        MakeMoney();
        Pollute();

        var type = attrib.GetPollutionType();
        var emission = attrib.pollutionAttrib.emissionPerTurn;
        pollutionSum.AddPollution(type, emission);
    }

    public void GetReady()
    {
        stateManager.AddEndPlayerTurnEventListener(GetOwnerID(), Operate);
        pollutionSum = transform.parent.parent.GetComponent<PollutionSum>();
    }
}
