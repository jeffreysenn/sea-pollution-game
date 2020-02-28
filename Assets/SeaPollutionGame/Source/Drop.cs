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

    // TODO(Xiaoyue Chen): Another class for canceling
    Transform oriParent;
    Vector3 oriPos;

    public void SetOriginalPos(Transform parent, Vector3 pos)
    {
        oriParent = parent;
        oriPos = pos;
    }

    void Start()
    {
        if (GetComponent<Factory>()) { type = EntityType.FACTORY; }
        else if (GetComponent<Filter>()) { type = EntityType.FILTER; }
        followMouse = GetComponent<FollowMouse>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) { DropEntity(); }
        if (Input.GetButtonDown("Fire2")) { CancelDrop(gameObject); }
    }

    void CancelDrop(GameObject obj)
    {
        Destroy(obj.GetComponent<FollowMouse>());
        Destroy(obj.GetComponent<Drop>());
        obj.transform.parent = oriParent;
        obj.transform.localPosition = oriPos;
        obj.AddComponent<Select>();
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
            var clone = Instantiate(gameObject);
            var clonePolluter = clone.GetComponent<Polluter>();
            clonePolluter.Copy(GetComponent<Polluter>());
            CancelDrop(clone);

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
