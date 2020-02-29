#define USE_OBJ_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttribData
{
    public Pollutant[] pollutantList;
    public Disaster[] disasterList;
    public PollutionAttrib[] environmentPollutionList;
    public FactoryAttrib[] factoryList;
    public FilterAttrib[] filterList;
}

public class AttribLoader : MonoBehaviour
{
    public EnvironmentPollution[] environmentPollutions;
    
    public AttribData attribData;
    
    void Awake()
    {
#if UNITY_WEBGL
        var data = Resources.Load<TextAsset>("TweakMe");
        var attribData = JsonUtility.FromJson<AttribData>(data.ToString());
#else
        var path = Application.dataPath + "/Resources/TweakMe.json";

        string data = System.IO.File.ReadAllText(path);

        attribData = JsonUtility.FromJson<AttribData>(data);
#endif

#if USE_OBJ_MENU
        var purchaseMenu = FindObjectOfType<PurchaseMenu>();
        var factoryAttribs = purchaseMenu.purchasables[0].polluterAttribs;
        foreach (var factoryAttrib in attribData.factoryList)
        {
            factoryAttribs.Add(factoryAttrib);
        }
        var filterAttribs = purchaseMenu.purchasables[1].polluterAttribs;
        foreach (var filterAttrib in attribData.filterList)
        {
            filterAttribs.Add(filterAttrib);
        }
#endif

        for(int i = 0; i != attribData.environmentPollutionList.Length; ++i)
        {
            if(i == environmentPollutions.Length || !environmentPollutions[i]) { break; }
            environmentPollutions[i].pollutionAttrib = attribData.environmentPollutionList[i];
        }

        var disasterManager = FindObjectOfType<DisasterManager>();
        disasterManager.SetDisasters(attribData.disasterList);

        var materialManager = FindObjectOfType<PollutantMaterialManager>();
        foreach(var pollutant in attribData.pollutantList)
        {
            materialManager.AddPollutant(pollutant.title, pollutant.color);
        }
    }
}
