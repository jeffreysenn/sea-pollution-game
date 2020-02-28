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

    void Awake()
    {
#if UNITY_WEBGL
        var data = Resources.Load<TextAsset>("TweakMe");
        var attribData = JsonUtility.FromJson<AttribData>(data.ToString());
#else
        var path = Application.dataPath + "/Resources/TweakMe.json";
        string data = System.IO.File.ReadAllText(path);
        var attribData = JsonUtility.FromJson<AttribData>(data);
#endif
        PurchaseMenu purchaseMenu = FindObjectOfType<PurchaseMenu>().GetComponent<PurchaseMenu>();
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

        for(int i = 0; i != attribData.environmentPollutionList.Length; ++i)
        {
            if(i == environmentPollutions.Length || !environmentPollutions[i]) { break; }
            environmentPollutions[i].pollutionAttrib = attribData.environmentPollutionList[i];
        }

        var disasterManager = FindObjectOfType<DisasterManager>().GetComponent<DisasterManager>();
        disasterManager.SetDisasters(attribData.disasterList);

        var materialManager = FindObjectsOfType<PollutantMaterialManager>()[0];
        foreach(var pollutant in attribData.pollutantList)
        {
            materialManager.AddPollutant(pollutant.title, pollutant.color);
        }
    }
}
