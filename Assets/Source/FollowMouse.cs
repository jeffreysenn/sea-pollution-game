using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        var screenPos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.y = transform.position.y;
        transform.position = worldPos;
    }
}
