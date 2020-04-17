using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 speed = new Vector2 { };
    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private float left = -0.2F;
    [SerializeField] private float right = 0.2F;
    [SerializeField] private float top = 0.2F;
    [SerializeField] private float bottom = -0.2F;


    Camera cam = null;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    //private void Update()
    //{
    //    var pos = transform.position;
    //    pos.x += Input.GetAxis("Horizontal") * speed.x * Time.deltaTime;
    //    pos.z += Input.GetAxis("Vertical") * speed.y * Time.deltaTime;
    //    transform.position = pos;

    //    cam.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
    //}

    private void LateUpdate()
    {
        cam.lensShift = new Vector2(0.1f, 0);
        //Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
        //cam.projectionMatrix = m;
    }

    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
}
