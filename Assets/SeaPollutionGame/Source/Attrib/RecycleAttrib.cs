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
        public string convertTo;
        public float conversionRate;
        public float maxConversion;
    }

    public Conversion[] conversions = null;
}