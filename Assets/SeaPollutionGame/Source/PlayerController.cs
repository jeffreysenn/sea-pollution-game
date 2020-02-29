#define USE_OBJ_MENU

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
        state = State.HOLDING;
        holdingPolluter = polluter;
    }

    public void CancelHold()
    {
        Destroy(holdingPolluter);
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
            if (validSpace && validSpace.ownerID == FindObjectsOfType<WorldStateManager>()[0].GetCurrentPlayerID())
            {
                validSpace.polluter = holdingPolluter;
                var targetPos = validSpace.transform.position;
                targetPos.y = holdingPolluter.transform.position.y;
                holdingPolluter.transform.position = targetPos;
                holdingPolluter.transform.parent = validSpace.transform;
                holdingPolluter.Activate();
                state = State.EMPTY;
                return true;
            }
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
                        var hits = MouseRaycastDownAll();
                        Polluter hitPolluter = null;
                        foreach(var hit in MouseRaycastDownAll()) {
                            hitPolluter = hit.transform.gameObject.GetComponent<Polluter>();
                            if (hitPolluter) {
                                Hold(hitPolluter);
                                break;
                            }
                        }
                    }
#endif
                    }
                break;
            case State.HOLDING:
                {
                    FollowMouse();
#if USE_OBJ_MENU
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
}
