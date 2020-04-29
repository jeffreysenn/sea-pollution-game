using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowParticleController : MonoBehaviour
{
    [SerializeField] private string pollutantName = "Emission";
    [SerializeField] private float maxPollution = 40;
    private ParticleSystem particleSystem = null;
    private ParticleSystem.MinMaxCurve rateOverTime = new ParticleSystem.MinMaxCurve { };

    public void SetStartPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetEndPos(Vector3 pos)
    {
        var col = GetComponentInChildren<FlowParticleCollisionTag>();
        col.transform.position = pos;
    }


    private void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rateOverTime = particleSystem.emission.rateOverTime;
        var emission = particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve { };
        var flow = GetComponentInParent<Flow>();
        var inputEvent = flow.GetInputEvent();
        inputEvent.AddListener(UpdateParticle);
    }

    private void UpdateParticle(PollutionMap pollutionMap)
    {
        float target = 0;
        pollutionMap.TryGetValue(pollutantName, out target);
        float theta = target / maxPollution;
        var emission = particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rateOverTime.constantMin * theta,
                                                               rateOverTime.constantMax * theta);
    }
}
