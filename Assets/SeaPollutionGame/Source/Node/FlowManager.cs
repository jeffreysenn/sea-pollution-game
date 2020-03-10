using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private Flow[] _flows;
    public Flow[] flows { get { return _flows; } }

    private void Awake()
    {
        _flows = FindObjectsOfType<Flow>();
    }
}
