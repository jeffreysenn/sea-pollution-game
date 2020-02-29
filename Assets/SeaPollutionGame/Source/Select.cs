using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    // TODO(Xiaoyue Chen): reduce coupling
    WorldStateManager stateManager = null;
    float polluterPrice = 0;

    void Start()
    {
        stateManager = WorldStateManager.FindWorldStateManager();
        var polluter = GetComponent<Polluter>();
        polluterPrice = polluter.GetAttrib().economicAttrib.price;
    }

    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (stateManager.GetMoney(stateManager.GetCurrentPlayerID()) < polluterPrice) return;

            var drop = gameObject.AddComponent<Drop>();
            drop.SetOriginalPos(transform.parent, transform.localPosition);
            gameObject.AddComponent<FollowMouse>();
            Destroy(this);
        }   
    }
}
