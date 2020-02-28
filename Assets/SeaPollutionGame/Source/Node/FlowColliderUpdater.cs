using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowColliderUpdater : MonoBehaviour
{
    public float colliderWidth = 0.6f;
    public float colliderHeight = 1;

    public void UpdateFlowCollider()
    {
        var flow = GetComponent<Flow>();
        if (flow)
        {
            var originPos = flow.GetInNode().transform.position;
            var targetPos = flow.GetOutNode().transform.position;
            var delta = targetPos - originPos;
            float distance = delta.magnitude;
            flow.transform.position = originPos + delta / 2;
            flow.transform.right = delta.normalized;
            
            var boxCollider = flow.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(distance, colliderHeight, colliderWidth);
            boxCollider.center = Vector3.zero;
        }
    }

}
