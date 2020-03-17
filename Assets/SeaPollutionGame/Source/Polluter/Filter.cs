using System.Collections.Generic;
using UnityEngine;

public class Filter : Polluter
{
    public override void Mulfunction()
    {
        GetPollutionMap().Clear();
    }
}