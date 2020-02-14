using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : MonoBehaviour
{
    PollutionMap pollutionMap = new PollutionMap { };

    public void AddPollution(string pollutantName, float val) {
        if (!pollutionMap.ContainsKey(pollutantName)) { pollutionMap.Add(pollutantName, 0); }
        pollutionMap[pollutantName] += val;

        var filterSpace = transform.parent.GetComponent<FilterSpace>();
        filterSpace.UpdatePollution(pollutionMap);
        var drawPollutionSumDes = GetComponent<DrawDescription>();
        drawPollutionSumDes.SetDescription("Pollution sum:\n" + pollutionMap.GetDescription());
    }

    public float GetPollution(string pollutantName) { return pollutionMap[pollutantName]; }
}
