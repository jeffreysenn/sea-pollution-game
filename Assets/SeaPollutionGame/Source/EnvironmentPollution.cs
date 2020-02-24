using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPollution : MonoBehaviour
{
    public PollutionAttrib pollutionAttrib;

    void Start()
    {
        var pollutionSum = transform.parent.GetComponent<PollutionSum>();
        foreach (var emission in pollutionAttrib.emissions)
        {
            pollutionSum.AddPollution(emission.pollutantName, emission.emissionPerTurn);
        }
    }
}
