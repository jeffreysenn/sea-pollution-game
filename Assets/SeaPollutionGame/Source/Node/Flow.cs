using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PutDir
{
    IN = 0, OUT = 1,
}

public class PutDirUtil
{
    static public PutDir[] GetPutDirs()
    {
        return new PutDir[] { PutDir.IN, PutDir.OUT };
    }

    static public PutDir GetOpposite(PutDir dir)
    {
        return (PutDir)(((int)dir + 1) % 2);
    }
}

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

    public void SetNode(PutDir type, Node node)
    {
        ref var n = ref GetNodeRef(type);
        n = node;
    }

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

    private ref Node GetNodeRef(PutDir type)
    {
        if (type == PutDir.IN) { return ref inNode; }
        if (type == PutDir.OUT) { return ref outNode; }
        return ref inNode;
    }

    public Node GetNode(PutDir type)
    {
        return GetNodeRef(type);
    }

    public PollutionMap GetPollutionMap() { return pollutionMap; }

}
