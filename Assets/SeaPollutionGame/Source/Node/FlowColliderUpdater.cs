using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowColliderUpdater : MonoBehaviour
{
    public float colliderWidth = 0.6f;
    public float colliderHeight = 0.6f;
    public float nodeRadius = 0.5f;

    public void UpdateFlowCollider()
    {
        var flow = GetComponent<Flow>();
        if (flow)
        {
            var originPos = flow.GetNode(PutDir.IN).transform.position;
            var targetPos = flow.GetNode(PutDir.OUT).transform.position;
            var delta = targetPos - originPos;
            float distance = delta.magnitude;
            flow.transform.position = originPos + delta / 2;
            flow.transform.right = delta.normalized;
            
            var boxCollider = flow.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(distance - 2 * nodeRadius, colliderHeight, colliderWidth);
            boxCollider.center = Vector3.zero;
        }
    }

}
