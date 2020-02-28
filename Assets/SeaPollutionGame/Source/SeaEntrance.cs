using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaEntrance : Node
{
    public List<int> ownerIDs = new List<int> { };

    void ReportPollution()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        var devidedMap = Util.DivideMap(GetPollutionMap(), ownerIDs.Count);
        foreach (int id in ownerIDs)
        {
            stateManager.AddPollution(id, PollutionMapType.NET, devidedMap);
        }
    }

    public override void Start()
    {
        base.Start();
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        foreach (int id in ownerIDs)
        {
            stateManager.AddEndPlayerTurnEventListener(id, ReportPollution);
        }
    }
}
