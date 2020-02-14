using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : MonoBehaviour
{
    public int ownerID = -1;
    PollutionMap pollutionMap = new PollutionMap { };

    public void AddPollution(string pollutantName, float val) {
        if (!pollutionMap.ContainsKey(pollutantName)) { pollutionMap.Add(pollutantName, 0); }
        pollutionMap[pollutantName] += val;

        var filterSpace = transform.parent.GetComponent<FilterSpace>();
        filterSpace.UpdatePollution(pollutionMap);
        var drawPollutionSumDes = GetComponent<DrawDescription>();
        drawPollutionSumDes.SetDescription("Pollution sum(per turn):\n" + pollutionMap.GetDescription());
    }

    public float GetPollution(string pollutantName) { return pollutionMap[pollutantName]; }

    void ReportPollution()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddProducedPollution(ownerID, pollutionMap);
    }

    void Start()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddEndPlayerTurnEventListener(ownerID, ReportPollution);
    }
}
