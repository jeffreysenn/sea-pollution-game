using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlowManager : MonoBehaviour
{
    private Flow[] _flows;
    public Flow[] flows { get { return _flows; } }

    Dictionary<Flow, VisibilityController> flowVisibilityDictionary = null;

    private bool isShown = false;
    public event Action<bool> OnDisplay;

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
        if (isShown) return;

        foreach(Flow f in flowVisibilityDictionary.Keys)
        {
            if(flowVisibilityDictionary[f] != null)
            {
                flowVisibilityDictionary[f].SetVisible(true);
            }
        }

        isShown = true;
        OnDisplay?.Invoke(isShown);
    }

    public void Hide()
    {
        if (!isShown) return;

        foreach (Flow f in flowVisibilityDictionary.Keys)
        {
            if (flowVisibilityDictionary[f] != null)
            {
                flowVisibilityDictionary[f].SetVisible(false);
            }
        }

        isShown = false;
        OnDisplay?.Invoke(isShown);
    }
}
