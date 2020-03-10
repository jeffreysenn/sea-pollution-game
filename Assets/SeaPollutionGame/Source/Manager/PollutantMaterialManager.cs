using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutantMaterialManager : MonoBehaviour
{
    Dictionary<string, Material> materialMap = new Dictionary<string, Material> { };

    public void AddPollutant(string name, Color32 color)
    {
        var mat = new Material(Shader.Find("Chart/Canvas/Solid"));
        mat.color = color;
        materialMap[name] = mat;
    }

    public Material GetMaterial(string name) { return materialMap[name]; }
}
