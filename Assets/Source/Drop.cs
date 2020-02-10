using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    enum EntityType
    {
        NONE,
        FACTORY,
        FILTER,
    };

    EntityType type = EntityType.NONE;
    FollowMouse followMouse = null;

    void Start()
    {
        if (GetComponent<Factory>()) { type = EntityType.FACTORY; }
        else if (GetComponent<Filter>()) { type = EntityType.FILTER; }
        followMouse = GetComponent<FollowMouse>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) { DropEntity(); }
        if (Input.GetButtonDown("Fire3")) { }
    }

    void DropEntity()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, new Vector3(0, -1, 0));
        Space validSpace = null;

        foreach (var hit in hits)
        {
            var hitObj = hit.transform.gameObject;
            switch (type)
            {
                case EntityType.FACTORY:
                    validSpace = hitObj.GetComponent<FactorySpace>();
                    break;
                case EntityType.FILTER:
                    validSpace = hitObj.GetComponent<FilterSpace>();
                    break;
            }
            if (validSpace) { break; }
        }


        if (validSpace && validSpace.ownerID == 
            WorldStateManager.FindWorldStateManager().GetCurrentPlayerID())
        {
            Destroy(followMouse);
            var polluter = GetComponent<Polluter>();
            validSpace.polluter = polluter;
            var targetPos = validSpace.transform.position;
            targetPos.y = transform.position.y;
            transform.position = targetPos;
            transform.parent = validSpace.transform;
            polluter.Activate();
            Destroy(this);
        }
    }
}
