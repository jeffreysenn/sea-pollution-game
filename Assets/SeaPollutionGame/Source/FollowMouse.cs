using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float boardPlane = 0.0f;
    void Update()
    {
        var camera = Camera.main;
        var mousePos = Input.mousePosition;
        mousePos.z = camera.transform.position.y - boardPlane;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //worldPos.y = transform.position.y;
        Debug.Log(worldPos);
        transform.position = worldPos;
    }
}
