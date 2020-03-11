using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEmissionManager : MonoBehaviour
{
    public CountryAttrib[] CountryAttribs = null;

    public void SetCountry(string country)
    {
        var countryAttrib = System.Array.Find(CountryAttribs, attrib => attrib.countryName == country);
        SetBaseEmission(countryAttrib.baseEmission);
    }

    private void SetBaseEmission(BaseEmissionAttrib baseEmission)
    {
        var boardPieces = FindObjectsOfType<BoardPiece>();
        foreach (var piece in boardPieces)
        {
            PollutionAttrib attrib;
            switch (piece.GetPlaceType())
            {
                case PlaceType.URBAN:
                    attrib = baseEmission.urban;
                    break;
                case PlaceType.AGRICULTURE:
                    attrib = baseEmission.agriculture;
                    break;
                case PlaceType.FOREST:
                    attrib = baseEmission.forest;
                    break;
                case PlaceType.OCEAN:
                    attrib = baseEmission.ocean;
                    break;
                default:
                    continue;
            }
            piece.SetBaseEmission(attrib);
        }
    }
}
