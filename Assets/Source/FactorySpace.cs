using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySpace : Space
{
    public PollutionSum GetPollutionSum()
    {
        return transform.parent.GetComponent<PollutionSum>();
    }
}
