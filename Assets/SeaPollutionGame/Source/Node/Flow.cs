using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flow : MonoBehaviour, IPollutionMapOwner
{
    public class PollutionEvent : UnityEvent<PollutionMap> { }
    private PollutionEvent inputEvent = new PollutionEvent { };
    [SerializeField] private float minDeltaInput = 0.01f;

    public PollutionEvent GetInputEvent()
    {
        return inputEvent;
    }

    [SerializeField] Node inNode = null;
    [SerializeField] Node outNode = null;
    PollutionMap pollutionMap = new PollutionMap { };

    public void SetInNode(Node node) { inNode = node; }
    public void ClearInNode() { inNode = null; }
    public void SetOutNode(Node node) { outNode = node; }
    public void ClearOutNode() { outNode = null; }

    public void Input(PollutionMap map)
    {
        if (Mathf.Abs(Util.SumMap(pollutionMap - map)) >= minDeltaInput)
        {
            pollutionMap = new PollutionMap(map);
            inputEvent.Invoke(map);

            OutPut();
        }
    }

    public void OutPut()
    {
        if (outNode) { outNode.Input(this, pollutionMap); }
    }

    public Node GetInNode() { return inNode; }
    public Node GetOutNode() { return outNode; }
    public PollutionMap GetPollutionMap() { return pollutionMap; }

    public void OnDisable()
    {
        if (inNode) { inNode.RemoveOutFlow(this); }
        if (outNode) { outNode.RemoveInFlow(this); }
    }
}
