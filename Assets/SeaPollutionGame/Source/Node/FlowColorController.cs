using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowColorController : MonoBehaviour
{
    [SerializeField] private float maxPollution = 40;
    [SerializeField] private AnimationCurve[] curves = new AnimationCurve[4];

    private LineRenderer lineRenderer = null;
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();

        var flow = GetComponentInChildren<Flow>();
        var inputEvent = flow.GetInputEvent();
        inputEvent.AddListener(LerpColor);

        LerpColor(flow.GetPollutionMap());
    }

    void LerpColor(PollutionMap map)
    {
        float sum = Util.SumMap(map);
        float time = sum / maxPollution;
        time = time > 1 ? 1 : time;
        float[] rgba = new float[4];
        for (int i = 0; i != 4; ++i)
        {
            rgba[i] = curves[i].Evaluate(time);
        }
        var color = new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
