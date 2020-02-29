#define USE_OBJ_MENU
#define CONTROLLER_HANDLES_DROP
#define CONTROLLER_HANDLES_CANCEL_HOLD
#define CONTROLLER_HANDLES_REMOVE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        EMPTY,
        HOLDING,
    }

    [SerializeField] private float boardPlaneY = 0;

    State state = State.EMPTY;
    Polluter holdingPolluter = null;

    public State GetState() { return state; }

    public void Hold(Polluter polluter)
    {
        if (polluter.GetSpace() != null) return;
        state = State.HOLDING;
        holdingPolluter = polluter;
    }

    public void CancelHold()
    {
        Destroy(holdingPolluter.gameObject);
        state = State.EMPTY;
    }

    public bool TryDrop()
    {
        var hits = MouseRaycastDownAll();
        Space validSpace = null;

        foreach (var hit in hits)
        {
            var hitObj = hit.transform.gameObject;
            if (holdingPolluter.GetComponent<Factory>()) { validSpace = hitObj.GetComponent<FactorySpace>(); }
            else if (holdingPolluter.GetComponent<Filter>()) { validSpace = hitObj.GetComponent<FilterSpace>(); }
            if (validSpace && validSpace.ownerID == FindObjectOfType<WorldStateManager>().GetCurrentPlayerID())
            {
                validSpace.polluter = holdingPolluter;

                var targetPos = validSpace.transform.position;
                targetPos.y = holdingPolluter.transform.position.y;

                holdingPolluter.transform.parent = validSpace.transform;

                holdingPolluter.transform.position = targetPos;
                holdingPolluter.transform.rotation = Quaternion.Euler(0, 30, 0);
                holdingPolluter.transform.localScale = Vector3.one;
                
                var textMesh = holdingPolluter.GetIdTextMesh();
                textMesh.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                holdingPolluter.SetIdText(holdingPolluter.polluterId.ToString());
                
                holdingPolluter.Activate();

                state = State.EMPTY;

                return true;
            }
        }

        return false;
    }

    public bool TryRemove(Polluter polluter)
    {
        if (polluter.CanRemove())
        {
            Destroy(polluter.gameObject);
            return true;
        }
        return false;
    }

    private void Update()
    {
        switch (state)
        {
            case State.EMPTY:
                {
#if USE_OBJ_MENU
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Polluter hitPolluter = GetMouseHitPolluter();
                        if (hitPolluter) { Hold(hitPolluter); }
                    }
#endif
#if CONTROLLER_HANDLES_REMOVE
                    if (Input.GetButtonDown("Fire2"))
                    {
                        var hitPolluter = GetMouseHitPolluter();
                        if (hitPolluter) { TryRemove(hitPolluter); }
                    }
#endif
                }
                break;
            case State.HOLDING:
                {
                    FollowMouse();

#if CONTROLLER_HANDLES_CANCEL_HOLD
                    if (Input.GetButtonDown("Fire2"))
                    {
                        CancelHold();
                    }
#endif

#if CONTROLLER_HANDLES_DROP
                    if (Input.GetButtonDown("Fire1"))
                    {
                        TryDrop();
                    }
#endif
                }
                break;
        }
    }

    private void FollowMouse()
    {
        holdingPolluter.transform.position = GetWorldMousePos();
    }

    private Vector3 GetWorldMousePos()
    {
        var camera = Camera.main;
        var mousePos = Input.mousePosition;
        mousePos.z = camera.transform.position.y - boardPlaneY;
        return camera.ScreenToWorldPoint(mousePos);
    }

    private RaycastHit[] MouseRaycastDownAll()
    {
        var raycastOrigin = GetWorldMousePos();
        raycastOrigin.y += 100;
        return Physics.RaycastAll(raycastOrigin, Vector3.down, 9999);
    }

    private Polluter GetMouseHitPolluter()
    {
        foreach (var hit in MouseRaycastDownAll())
        {
            var hitPolluter = hit.transform.gameObject.GetComponent<Polluter>();
            if (hitPolluter)
            {
                return hitPolluter;
            }
        }
        return null;
    }
}
