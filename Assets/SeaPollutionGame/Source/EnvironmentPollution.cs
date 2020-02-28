using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPollution : MonoBehaviour
{
    public PollutionAttrib pollutionAttrib;
    public PollutionSum pollutionSum = null;

    void Start()
    {
        if (pollutionSum)
        {
            pollutionSum.SetLocalPollution(new PollutionMap(pollutionAttrib.emissions));
        }
    }
}
