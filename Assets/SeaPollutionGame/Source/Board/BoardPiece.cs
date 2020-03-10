using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour
{
    [SerializeField] private PlaceType type = PlaceType.NONE;

    public void SetBaseEmission(PollutionAttrib attrib)
    {
        var baseEmission = new PollutionMap(attrib.emissions);
        var baseEmissionComps = GetComponentsInChildren<BaseEmission>();
        var validBaseEmissionComps = new List<BaseEmission>();
        foreach (var comp in baseEmissionComps)
        {
            if (comp.gameObject.activeInHierarchy) { validBaseEmissionComps.Add(comp); }
        }
        var divided = Util.DivideMap(baseEmission, baseEmissionComps.Length);
        foreach (var comp in validBaseEmissionComps)
        {
            comp.SetLocalPollution(divided);
        }
    }

    public PlaceType GetPlaceType() { return type; }
}
