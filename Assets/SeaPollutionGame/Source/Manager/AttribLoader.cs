﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseEmissionAttrib
{
    public PollutionAttrib urban, agriculture, forest, ocean;
}

/// <summary>
/// A data structure storing the data loaded from Json
/// </summary>
public struct AttribData
{
    public Pollutant[] pollutantList;
    public Disaster[] disasterList;
    public BaseEmissionAttrib baseEmission;
    public FactoryAttrib[] factoryList;
    public FilterAttrib[] filterList;
}

/// <summary>
/// Loads and stores attrib from Json
/// </summary>
public class AttribLoader : MonoBehaviour
{
    /// <summary>
    /// An intermediate class to load data from Json, and feed the data into AttribData class.
    /// Internal use only.
    /// </summary>
    [System.Serializable]
    public struct JsonData
    {
        [System.Serializable]
        public class PlacementJson
        {
            public bool urban = true, agriculture = true, forest = true, ocean = true;
            public PlaceType GetSignature()
            {
                var sig = PlaceType.NONE;
                if (ocean) { sig |= PlaceType.OCEAN; }
                if (urban) { sig |= PlaceType.URBAN; }
                if (agriculture) { sig |= PlaceType.AGRICULTURE; }
                if (forest) { sig |= PlaceType.FOREST; }
                return sig;
            }
        }

        [System.Serializable]
        public class PolluterJson
        {
            public string title = "Default Polluter";
            public EconomicAttrib economicAttrib = new EconomicAttrib { };
            public PollutionAttrib pollutionAttrib = new PollutionAttrib { };
            public VulnerabilityAttrib vulnerabilityAttrib = new VulnerabilityAttrib { };
            public PlacementJson placementAttrib = new PlacementJson { };

            public PolluterAttrib ToPolluterAttrib()
            {
                var r = new PolluterAttrib
                {
                    title = title,
                    economicAttrib = economicAttrib,
                    pollutionAttrib = pollutionAttrib,
                    vulnerabilityAttrib = vulnerabilityAttrib,
                    placementAttrib = new PlacementAttrib(placementAttrib.GetSignature())
                };
                return r;
            }
        }



        public Pollutant[] pollutantList;
        public Disaster[] disasterList;
        public BaseEmissionAttrib baseEmission;
        public PolluterJson[] factoryList;
        public PolluterJson[] filterList;

        public AttribData ToAttribData()
        {
            return new AttribData
            {
                pollutantList = pollutantList,
                disasterList = disasterList,
                baseEmission = baseEmission,
                factoryList = System.Array.ConvertAll(ToPolluterAttribs(factoryList), attrib => new FactoryAttrib(attrib)),
                filterList = System.Array.ConvertAll(ToPolluterAttribs(filterList), attrib => new FilterAttrib(attrib)),
            };
        }

        private PolluterAttrib[] ToPolluterAttribs(PolluterJson[] polluterJson)
        {
            var length = polluterJson.Length;
            var result = new PolluterAttrib[length];
            for (int i = 0; i != length; ++i)
            {
                var polluterAttrib = polluterJson[i].ToPolluterAttrib();
                result[i] = polluterAttrib;
            }
            return result;
        }
    }

    public AttribData attribData;

    void Awake()
    {
#if UNITY_WEBGL
        var data = Resources.Load<TextAsset>("TweakMe");
        var jsonData = JsonUtility.FromJson<JsonData>(data.text);
#else
        var path = Application.dataPath + "/Resources/TweakMe.json";
        var data = System.IO.File.ReadAllText(path);
        var jsonData = JsonUtility.FromJson<JsonData>(data);
#endif
        attribData = jsonData.ToAttribData();

        var baseEmission = attribData.baseEmission;
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

        var disasterManager = FindObjectOfType<DisasterManager>();
        disasterManager.SetDisasters(attribData.disasterList);

        var materialManager = FindObjectOfType<PollutantMaterialManager>();
        foreach (var pollutant in attribData.pollutantList)
        {
            materialManager.AddPollutant(pollutant.title, pollutant.color);
        }
    }
}