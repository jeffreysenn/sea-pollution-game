using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecycleAttrib
{
    [System.Serializable]
    public struct Conversion
    {
        public string pollutantName;
        public float conversionRate;
    }

    public Conversion[] conversions = null;
}