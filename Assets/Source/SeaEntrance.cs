using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaEntrance : MonoBehaviour
{
    PollutionMap pollutionMap = new PollutionMap { };

    public void SetPollutionMap(PollutionMap pollution)
    {
        pollutionMap = pollution;
        var drawDescrip = GetComponent<DrawDescription>();
        drawDescrip.SetDescription("Pollution into the sea:\n" + pollutionMap.GetDescription());
    }


}
