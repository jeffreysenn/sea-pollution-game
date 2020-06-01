using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [Header("Offset")]
    [SerializeField]
    private Vector2 hotSpot = Vector2.zero;
    [SerializeField]
    private Vector2 grabOffset = Vector2.zero;
    public Vector3 GetGrabOffset() { return grabOffset; }

    [Header("Textures")]
    [SerializeField]
    private Texture2D cursorDefault = null;
    [SerializeField]
    private Texture2D cursorOnClick = null;
    [SerializeField]
    private Texture2D cursorOnGrab = null;

    PlayerController playerController = null;

    private bool isGrabbing = false;
    private bool isClicking = false;
    private CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
    }

    void Start()
    {
        playerController = UIManager.Instance.playerController;
    }
    
    void Update()
    {
        if (!isGrabbing && playerController.GetState() == PlayerController.State.HOLDING)
        {
            Cursor.SetCursor(cursorOnGrab, hotSpot, cursorMode);
            isGrabbing = true;
        } else if (!isClicking && Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorOnClick, hotSpot, cursorMode);
            isClicking = true;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorDefault, hotSpot, cursorMode);
            isGrabbing = false;
            isClicking = false;
        }
    }
}
