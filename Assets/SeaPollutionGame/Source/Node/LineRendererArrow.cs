using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererArrow : MonoBehaviour
{
    [Tooltip("The percent of the line that is consumed by the arrowhead")]
    [Range(0, 1)]
    public float percentHead = 0.4f;
    public Vector3 origin;
    public Vector3 target;
    private LineRenderer cachedLineRenderer;

    [ContextMenu("UpdateArrow")]
    public void UpdateArrow()
    {
        if (cachedLineRenderer == null)
            cachedLineRenderer = this.GetComponent<LineRenderer>();
        cachedLineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f)
            , new Keyframe(0.999f - percentHead, 0.4f)  // neck of arrow
            , new Keyframe(1 - percentHead, 1f)  // max width of arrow head
            , new Keyframe(1, 0f));  // tip of arrow
        cachedLineRenderer.positionCount = 4;
        cachedLineRenderer.SetPositions(new Vector3[] {
              origin
              , Vector3.Lerp(origin, target, 0.999f - percentHead)
              , Vector3.Lerp(origin, target, 1 - percentHead)
              , target });
    }
}
