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

    public List<Flow> GetFlows(PutDir dir)
    {
        if(dir == PutDir.IN) { return inFlows; }
        if(dir == PutDir.OUT) { return outFlows; }
        Debug.Assert(false);
        return inFlows;
    }

    public void AddFlow(PutDir dir, Flow flow)
    {
        var flows = GetFlows(dir);
        flows.Add(flow);
    }

    public void RemoveFlow(PutDir dir, Flow flow)
    {
        var flows = GetFlows(dir);
        flows.Remove(flow);
    }

    public List<Flow> GetAllFlows()
    {
        var result = new List<Flow> { };
        result.AddRange(inFlows);
        result.AddRange(outFlows);
        return result;
    }


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

    public PollutionMap GetInputPollutionMap()
    {
        return Util.SumMap(inFlowPollutions);
    }

    public PollutionMap GetPollutionMap()
    {
        return GetInputPollutionMap() + localPollution;
    }
}
