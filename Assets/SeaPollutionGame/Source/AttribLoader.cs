using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data structure storing the data loaded from Json
/// </summary>
public struct AttribData
{
    public Pollutant[] pollutantList;
    public Disaster[] disasterList;
    public PollutionAttrib[] environmentPollutionList;
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
            public bool ocean = true, urban = true, agriculture = true, forest = true;
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
        public PollutionAttrib[] environmentPollutionList;
        public PolluterJson[] factoryList;
        public PolluterJson[] filterList;

        public AttribData ToAttribData()
        {
            return new AttribData
            {
                pollutantList = pollutantList,
                disasterList = disasterList,
                environmentPollutionList = environmentPollutionList,
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

    public EnvironmentPollution[] environmentPollutions;

    public AttribData attribData;

    void Awake()
    {
#if UNITY_WEBGL
        var data = Resources.Load<TextAsset>("TweakMe");
#else
        var path = Application.dataPath + "/Resources/TweakMe.json";
        var data = System.IO.File.ReadAllText(path);
#endif
        var jsonData = JsonUtility.FromJson<JsonData>(data);
        attribData = jsonData.ToAttribData();

        for (int i = 0; i != attribData.environmentPollutionList.Length; ++i)
        {
            if (i == environmentPollutions.Length || !environmentPollutions[i]) { break; }
            environmentPollutions[i].pollutionAttrib = attribData.environmentPollutionList[i];
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
