using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    private Space[] _spaces;
    public Space[] spaces { get { return _spaces; } }

    private void Awake()
    {
        _spaces = FindObjectsOfType<Space>();
    }
}
