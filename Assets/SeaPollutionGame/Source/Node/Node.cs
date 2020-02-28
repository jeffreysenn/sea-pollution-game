using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Node : MonoBehaviour, IPollutionMapOwner
{
    public class PollutionEvent : UnityEvent<Flow, PollutionMap> { }
    public PollutionEvent addInputEvent = new PollutionEvent { };
    public PollutionEvent removeInputEvent = new PollutionEvent { };
    public PollutionEvent setLocalPollutionEvent = new PollutionEvent { };
    public PollutionEvent clearLocalPollutionEvent = new PollutionEvent { };

    public PollutionEvent[] GetInputPollutionEvents()
    {
        return new PollutionEvent[] { addInputEvent, removeInputEvent };
    }

    public PollutionEvent[] GetAllPollutionEvents()
    {
        return new PollutionEvent[] { addInputEvent,
                                      removeInputEvent ,
                                      setLocalPollutionEvent,
                                      clearLocalPollutionEvent
        };
    }

    [SerializeField] private List<Flow> inFlows = new List<Flow> { };
    [SerializeField] private List<Flow> outFlows = new List<Flow> { };
    [SerializeField] private Dictionary<Flow, PollutionMap> inFlowPollutions = new Dictionary<Flow, PollutionMap> { };
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
        inFlowPollutions.Add(flow, new PollutionMap { });
    }

    public void RemoveInFlow(Flow flow) { inFlows.Remove(flow); }

    public void AddOutFlow(Flow flow) { outFlows.Add(flow); }
    public void RemoveOutFlow(Flow flow) { outFlows.Remove(flow); }

    public void SetLocalPollution(PollutionMap map)
    {
        localPollution.CopyAssign(map);
        setLocalPollutionEvent.Invoke(null, map);
    }

    public void ClearLocalPollution()
    {
        var clearedLocalPollution = new PollutionMap(localPollution);
        localPollution.Clear();
        clearLocalPollutionEvent.Invoke(null, clearedLocalPollution);
    }

    public PollutionMap GetLocalPollution() { return localPollution; }

    public void AddInput(Flow flow, PollutionMap map)
    {
        inFlowPollutions[flow] = new PollutionMap(map);
        addInputEvent.Invoke(flow, map);
    }

    public void RemoveInput(Flow flow)
    {
        var removedPollution = new PollutionMap(inFlowPollutions[flow]);
        inFlowPollutions.Remove(flow);
        removeInputEvent.Invoke(flow, removedPollution);
    }

    public void OutPut()
    {
        var sum = GetPollutionMap();
        var diveded = Util.DivideMap(sum, outFlows.Count);
        foreach (var flow in outFlows)
        {
            flow.SetInput(diveded);
        }
    }



    public virtual void Start()
    {
        foreach (var pollutionEvent in GetAllPollutionEvents())
        {
            pollutionEvent.AddListener((Flow, PollutionMap) => { OutPut(); });
        }
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
