using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 speed = new Vector2 { };
    [SerializeField] float zoomSpeed = 1.0f;

    Camera cam = null;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var pos = transform.position;
        pos.x += Input.GetAxis("Horizontal") * speed.x * Time.deltaTime;
        pos.z += Input.GetAxis("Vertical") * speed.y * Time.deltaTime;
        transform.position = pos;

        cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
    }
}
