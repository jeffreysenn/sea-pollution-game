using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionSum : MonoBehaviour
{
    Dictionary<Pollution.Type, float> pollutionMap = new Dictionary<Pollution.Type, float> { };

    public void AddPollution(Pollution.Type type, float val) { pollutionMap[type] += val; }
    public float GetPollution(Pollution.Type type) { return pollutionMap[type]; }
}
