using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttribData
{
    public Pollutant[] pollutantList;
    public PollutionAttrib[] environmentPollutionList;
    public FactoryAttrib[] factoryList;
    public FilterAttrib[] filterList;
}

public class AttribLoader : MonoBehaviour
{
    [SerializeField] EnvironmentPollution[] environmentPollutions;

    AttribData attribData = new AttribData { };

    void Awake()
    {
        var path = Application.dataPath + "/TweakMe.json";
        string data = System.IO.File.ReadAllText(path);
        attribData = JsonUtility.FromJson<AttribData>(data);

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
            environmentPollutions[i].pollutionAttrib = attribData.environmentPollutionList[i];
        }
    }
}
