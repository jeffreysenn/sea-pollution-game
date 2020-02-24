using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    List<Flow> inFlows = new List<Flow> { };
    List<Flow> outFlows = new List<Flow> { };
    Dictionary<Flow, PollutionMap> inFlowPollutions = new Dictionary<Flow, PollutionMap> { };

    public void AddInFlow(Flow flow)
    {
        inFlows.Add(flow);
        inFlowPollutions.Add(flow, new PollutionMap { });
    }

    public void RemoveInFlow(Flow flow) { inFlows.Remove(flow); }

    public void AddOutFlow(Flow flow) { outFlows.Add(flow); }
    public void RemoveOutFlow(Flow flow) { outFlows.Remove(flow); }

    public void AddInput(Flow flow, PollutionMap map) { inFlowPollutions[flow] = new PollutionMap(map); }
    public void RemoveInput(Flow flow) { inFlowPollutions.Remove(flow); }
    public void OutPut()
    {
        var sum = Util.SumMap(inFlowPollutions);
        var diveded = DivideMap(sum, outFlows.Count);
        foreach (var flow in outFlows)
        {
            flow.Input(diveded);
        }
    }

    PollutionMap DivideMap(PollutionMap map, float denominator)
    {
        var result = new PollutionMap(map);
        foreach (var pair in result)
        {
            float newVal = pair.Value / denominator;
            result[pair.Key] = newVal;
        }
        return result;
    }

    void OnDestroy()
    {
        foreach(var flow in inFlows) { flow.ClearOutNode(); }    
        foreach(var flow in outFlows) { flow.ClearInNode(); }    
    }
}
