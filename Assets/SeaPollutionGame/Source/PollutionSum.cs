using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : Node
{
    public int ownerID = -1;
    PollutionMap pollutionMap = new PollutionMap { };

    public float GetPollution(string pollutantName) { return pollutionMap[pollutantName]; }

    void ReportPollution()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddPollution(ownerID, PollutionMapType.PRODUCED, pollutionMap);
    }

    void Start()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddEndPlayerTurnEventListener(ownerID, ReportPollution);
    }
}
