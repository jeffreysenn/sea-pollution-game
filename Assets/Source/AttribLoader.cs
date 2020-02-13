using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttribData
{
    public Pollutant[] pollutionList;
    public FactoryAttrib[] factoryList;
    public FilterAttrib[] filterList;
}

public class AttribLoader : MonoBehaviour
{
    AttribData attribData = new AttribData { };
    void Start()
    {
        var path = Application.dataPath + "/TweakMe.json";
        string data = System.IO.File.ReadAllText(path);
        attribData = JsonUtility.FromJson<AttribData>(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
