using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttribCoordinator : MonoBehaviour
{
    void Awake()
    {
        var attribLoader = FindObjectOfType<AttribLoader>();
        var attribData = attribLoader.LoadLazy();

        var baseEmissionManager = FindObjectOfType<BaseEmissionManager>();
        baseEmissionManager.CountryAttribs = attribData.countryList;

        var disasterManager = FindObjectOfType<DisasterManager>();
        disasterManager.SetDisasters(attribData.disasterList);

        var materialManager = FindObjectOfType<PollutantMaterialManager>();
        materialManager.SetPollutants(attribData.pollutantList);
    }
}
