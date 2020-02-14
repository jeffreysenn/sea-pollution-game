using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawEnvironmentPollution : DrawDescription
{
    void Start()
    {
        var environmentPollution = GetComponent<EnvironmentPollution>();
        var pollutionAttrib = environmentPollution.pollutionAttrib;
        description = "Environment pollution:\n";
        description += pollutionAttrib.GetDiscription();
        descriptionText = GameObject.Find("ItemDescription").GetComponent<Text>();
    }
}
