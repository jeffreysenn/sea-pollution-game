using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySpace : Space {
    void ReportPollution()
    {
        if (!polluter) { return; }
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddPollution(ownerID, PollutionMapType.PRODUCED, GetLocalPollution());
    }

    public override void Start()
    {
        base.Start();
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddEndPlayerTurnEventListener(ownerID, ReportPollution);
    }
}
