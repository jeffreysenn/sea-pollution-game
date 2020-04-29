using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    [SerializeField] private bool visibleInGame = true;

    public void SetVisible(bool shouldVisible)
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach(var renderer in renderers)
        {
            renderer.enabled = shouldVisible;
        }
    }

    private void Start()
    {
        SetVisible(visibleInGame);
    }
}
