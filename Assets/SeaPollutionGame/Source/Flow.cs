using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow : MonoBehaviour
{
    Node inNode = null;
    Node outNode = null;
    PollutionMap pollutionMap = new PollutionMap { };

    public void SetInNode(Node node) { inNode = node; }
    public void SetOutNode(Node node) { outNode = node; }

    public void Input(PollutionMap map)
    {
        pollutionMap = new PollutionMap(map);
        OutPut();
    }

    public void ClearInput()
    {
        pollutionMap.Clear();
        if (outNode) { outNode.RemoveInput(this); }
    }

    public void OutPut()
    {
        if (outNode) { outNode.AddInput(this, pollutionMap); }
    }

    public Node GetInNode() { return inNode; }
    public Node GetOutNode() { return outNode; }
    public PollutionMap GetPollutionMap() { return pollutionMap; }
}
