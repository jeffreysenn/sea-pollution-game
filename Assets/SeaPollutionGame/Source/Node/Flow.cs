using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flow : MonoBehaviour, IPollutionMapOwner
{
    public class PollutionEvent : UnityEvent<PollutionMap> { }
    public PollutionEvent setInputEvent = new PollutionEvent { };
    public PollutionEvent clearInputEvent = new PollutionEvent { };
    [SerializeField] private float minDeltaInput = 0.01f;

    public PollutionEvent[] GetAllPollutionEvents()
    {
        return new PollutionEvent[] { setInputEvent, clearInputEvent };
    }

    [SerializeField] Node inNode = null;
    [SerializeField] Node outNode = null;
    PollutionMap pollutionMap = new PollutionMap { };

    public void SetInNode(Node node) { inNode = node; }
    public void ClearInNode() { inNode = null; }
    public void SetOutNode(Node node) { outNode = node; }
    public void ClearOutNode() { outNode = null; }

    public void SetInput(PollutionMap map)
    {
        if (Mathf.Abs(Util.SumMap(pollutionMap - map)) >= minDeltaInput)
        {
            pollutionMap = new PollutionMap(map);
            setInputEvent.Invoke(map);
        }
    }

    public void ClearInput()
    {
        var clearedInput = new PollutionMap(pollutionMap);
        pollutionMap.Clear();
        if (outNode) { outNode.RemoveInput(this); }
        clearInputEvent.Invoke(clearedInput);
    }

    public void OutPut()
    {
        if (outNode) { outNode.AddInput(this, pollutionMap); }
    }

    public Node GetInNode() { return inNode; }
    public Node GetOutNode() { return outNode; }
    public PollutionMap GetPollutionMap() { return pollutionMap; }

    private void Start()
    {
        foreach (var pollutionEvent in GetAllPollutionEvents())
        {
            pollutionEvent.AddListener((PollutionMap) => { OutPut(); });
        }
    }

    public void OnDisable()
    {
        if (inNode) { inNode.RemoveOutFlow(this); }
        if (outNode) { outNode.RemoveInFlow(this); }
    }
}
