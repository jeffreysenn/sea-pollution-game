using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private Flow[] _flows;
    public Flow[] flows { get { return _flows; } }

    Dictionary<Flow, VisibilityController> flowVisibilityDictionary = null;

    private void Awake()
    {
        flowVisibilityDictionary = new Dictionary<Flow, VisibilityController>();

        _flows = FindObjectsOfType<Flow>();

        foreach(Flow f in _flows)
        {
            VisibilityController visibility = f.GetComponentInChildren<VisibilityController>();

            flowVisibilityDictionary.Add(f, visibility);
        }
    }

    public void Show()
    {
        foreach(Flow f in flowVisibilityDictionary.Keys)
        {
            if(flowVisibilityDictionary[f] != null)
            {
                flowVisibilityDictionary[f].SetVisible(true);
            }
        }
    }

    public void Hide()
    {
        foreach (Flow f in flowVisibilityDictionary.Keys)
        {
            if (flowVisibilityDictionary[f] != null)
            {
                flowVisibilityDictionary[f].SetVisible(false);
            }
        }
    }
}
