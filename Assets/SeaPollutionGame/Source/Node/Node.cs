using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Node : MonoBehaviour, IPollutionMapOwner
{
    public class PollutionEvent : UnityEvent<Flow, PollutionMap> { }
    public PollutionEvent inputEvent = new PollutionEvent { };
    public PollutionEvent localPollutionEvent = new PollutionEvent { };

    public PollutionEvent GetInputEvent() { return inputEvent; }

    public PollutionEvent[] GetAllPollutionEvents()
    {
        return new PollutionEvent[] { inputEvent,
                                      localPollutionEvent,
        };
    }

    [SerializeField] private List<Flow> inFlows = new List<Flow> { };
    [SerializeField] private List<Flow> outFlows = new List<Flow> { };
    private Dictionary<Flow, PollutionMap> inFlowPollutions = new Dictionary<Flow, PollutionMap> { };
    private PollutionMap localPollution = new PollutionMap { };

    public List<Flow> GetInFlows() { return inFlows; }
    public List<Flow> GetOutFlows() { return outFlows; }
    public List<Flow> GetAllFlows()
    {
        var result = new List<Flow> { };
        result.AddRange(inFlows);
        result.AddRange(outFlows);
        return result;
    }

    public void AddInFlow(Flow flow)
    {
        inFlows.Add(flow);
    }

    public void RemoveInFlow(Flow flow) { inFlows.Remove(flow); }

    public void AddOutFlow(Flow flow) { outFlows.Add(flow); }
    public void RemoveOutFlow(Flow flow) { outFlows.Remove(flow); }

    public void SetLocalPollution(PollutionMap map)
    {
        localPollution.CopyAssign(map);
        localPollutionEvent.Invoke(null, map);

        OutPut();
    }

    public PollutionMap GetLocalPollution() { return localPollution; }

    public void Input(Flow flow, PollutionMap map)
    {
        if (!inFlowPollutions.ContainsKey(flow)) { inFlowPollutions.Add(flow, new PollutionMap { }); }

        inFlowPollutions[flow].CopyAssign(map);
        inputEvent.Invoke(flow, map);

        OutPut();
    }

    public void OutPut()
    {
        var sum = GetPollutionMap();
        var diveded = Util.DivideMap(sum, outFlows.Count);
        foreach (var flow in outFlows)
        {
            flow.Input(diveded);
        }
    }

    public void OnDisable()
    {
        foreach (var flow in inFlows) { flow.ClearOutNode(); }
        foreach (var flow in outFlows) { flow.ClearInNode(); }
    }

    public PollutionMap GetInputPollutionMap()
    {
        return Util.SumMap(inFlowPollutions);
    }

    public PollutionMap GetPollutionMap()
    {
        return GetInputPollutionMap() + localPollution;
    }
}
