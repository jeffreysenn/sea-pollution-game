using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleInGame : MonoBehaviour
{
    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }
    }
}
