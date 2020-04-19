using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxBlender : MonoBehaviour
{
    public Material skyboxMaterial = null;

    [Range(0f, 1f)]
    public float blend = 0f;

    private void Update()
    {
        if(blend <= 1f)
        {
            blend += 0.0f * Time.deltaTime;
            skyboxMaterial.SetFloat("_Blend", blend);
        } else
        {
            blend -= 0.0f * Time.deltaTime;
            skyboxMaterial.SetFloat("_Blend", blend);
        }
    }
}
