using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A data structure storing the data loaded from Json
/// </summary>
public class AttribData
{
    public Pollutant[] pollutantList;
    public Disaster[] disasterList;
    public CountryAttrib[] countryList;
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
            public VisualAttrib visualAttrib = new VisualAttrib { };

            public PolluterAttrib ToPolluterAttrib()
            {
                var r = new PolluterAttrib
                {
                    title = title,
                    economicAttrib = economicAttrib,
                    pollutionAttrib = pollutionAttrib,
                    vulnerabilityAttrib = vulnerabilityAttrib,
                    placementAttrib = new PlacementAttrib(placementAttrib.GetSignature()),
                    visualAttrib = visualAttrib
                };
                return r;
            }
        }



        public Pollutant[] pollutantList;
        public Disaster[] disasterList;
        public CountryAttrib[] countryList;
        public PolluterJson[] factoryList;
        public PolluterJson[] filterList;

        public AttribData ToAttribData()
        {
            return new AttribData
            {
                pollutantList = pollutantList,
                disasterList = disasterList,
                countryList = countryList,
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

    public AttribData attribData = null;

    public AttribData LoadLazy()
    {
        if (attribData == null)
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
        }

        return attribData;
    }
}
