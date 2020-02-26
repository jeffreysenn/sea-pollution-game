using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Node : MonoBehaviour, IPollutionMapOwner
{
    [SerializeField] List<Flow> inFlows = new List<Flow> { };
    [SerializeField] List<Flow> outFlows = new List<Flow> { };
    [SerializeField] Dictionary<Flow, PollutionMap> inFlowPollutions = new Dictionary<Flow, PollutionMap> { };
    PollutionMap localPollution = new PollutionMap { };

    public void AddInFlow(Flow flow)
    {
        inFlows.Add(flow);
        inFlowPollutions.Add(flow, new PollutionMap { });
    }

    public void RemoveInFlow(Flow flow) { inFlows.Remove(flow); }

    public void AddOutFlow(Flow flow) { outFlows.Add(flow); }
    public void RemoveOutFlow(Flow flow) { outFlows.Remove(flow); }

    public void SetLocalPollution(PollutionMap map) { localPollution = new PollutionMap(map); OutPut(); }
    public void ClearLocalPollution() { localPollution.Clear(); OutPut(); }

    public void AddInput(Flow flow, PollutionMap map) { inFlowPollutions[flow] = new PollutionMap(map); OutPut(); }
    public void RemoveInput(Flow flow) { inFlowPollutions.Remove(flow); OutPut(); }
    public void OutPut()
    {
        var sum = GetPollutionMap();
        var diveded = DivideMap(sum, outFlows.Count);
        foreach (var flow in outFlows)
        {
            flow.Input(diveded);
        }
    }

    PollutionMap DivideMap(PollutionMap map, float denominator)
    {
        var result = new PollutionMap(map);
        foreach (var key in result.Keys.ToList())
        {
            result[key] /= denominator;
        }
        return result;
    }

    public void OnDisable()
    {
        foreach (var flow in inFlows) { flow.ClearOutNode(); }
        foreach (var flow in outFlows) { flow.ClearInNode(); }
    }

    public PollutionMap GetPollutionMap()
    {
        var sum = Util.SumMap(inFlowPollutions);
        sum += localPollution;
        return sum;
    }
}
