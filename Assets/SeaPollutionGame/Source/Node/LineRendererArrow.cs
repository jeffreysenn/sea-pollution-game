using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererArrow : MonoBehaviour
{
    [Range(0, 1)]
    public float percentHead = 0.4f;
    public GameObject origin = null;
    public GameObject target = null;
    private LineRenderer cachedLineRenderer = null;

    public void UpdateArrow()
    {
        if(!(origin && target)) { return; }
        if (cachedLineRenderer == null)
            cachedLineRenderer = GetComponent<LineRenderer>();
        cachedLineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f)
            , new Keyframe(0.999f - percentHead, 0.4f)  // neck of arrow
            , new Keyframe(1 - percentHead, 1f)  // max width of arrow head
            , new Keyframe(1, 0f));  // tip of arrow
        cachedLineRenderer.positionCount = 4;
        var originPos = origin.transform.position;
        var targetPos = target.transform.position;
        cachedLineRenderer.SetPositions(new Vector3[] {
              originPos
              , Vector3.Lerp(originPos, targetPos, 0.999f - percentHead)
              , Vector3.Lerp(originPos, targetPos, 1 - percentHead)
              , targetPos });
    }
}
