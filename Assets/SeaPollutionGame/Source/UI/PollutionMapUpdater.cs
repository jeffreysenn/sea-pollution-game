using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionMapUpdater : MonoBehaviour
{
    IPollutionMapOwner mapOwner = null;
    DrawDescription drawDescription = null;

    void Start()
    {
        mapOwner = GetComponent<IPollutionMapOwner>();
        drawDescription = GetComponent<DrawDescription>();
    }

    void Update()
    {
        drawDescription.SetPollutionMap(mapOwner.GetPollutionMap());
    }
}
