using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A data structure storing the data loaded from Json
/// </summary>
public class AttribData
{
    public ScoreWeight scoreWeight = new ScoreWeight { };
    public Pollutant[] pollutantList;
    public Disaster[] disasterList;
    public CountryAttrib[] countryList;
    public Goal[] goalList;
    public FactoryAttrib[] factoryList;
    public FilterAttrib[] filterList;
    public RecyclerAttrib[] recyclerList;
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
    public struct PlacementWrapper { public PlacementJson placementAttrib; }

    [System.Serializable]
    public struct PlacementList { public PlacementWrapper[] list; }

    public interface IPolluterAttribList { PolluterAttrib[] GetPolluterAttribs(); }

    [System.Serializable]
    public struct PollutantList { public Pollutant[] pollutantList; }
    [System.Serializable]
    public struct DisasterList { public Disaster[] disasterList; }
    [System.Serializable]
    public struct CountryList { public CountryAttrib[] countryList; }
    [System.Serializable]
    public struct GoalList { public Goal[] list; }
    [System.Serializable]
    public struct FactoryList : IPolluterAttribList
    {
        public FactoryAttrib[] list;

        public PolluterAttrib[] GetPolluterAttribs()
        {
            return System.Array.ConvertAll(list, attrib => (PolluterAttrib)attrib);
        }
    }
    [System.Serializable]
    public struct FilterList : IPolluterAttribList
    {
        public FilterAttrib[] list;
        public PolluterAttrib[] GetPolluterAttribs()
        {
            return System.Array.ConvertAll(list, attrib => (PolluterAttrib)attrib);
        }
    }
    [System.Serializable]
    public struct RecyclerList : IPolluterAttribList
    {
        public RecyclerAttrib[] list;
        public PolluterAttrib[] GetPolluterAttribs()
        {
            return System.Array.ConvertAll(list, attrib => (PolluterAttrib)attrib);
        }
    }

    public enum FileFor
    {
        SCORE_WEIGHT,
        POLLUTANT,
        DISASTER,
        COUNTRY,
        GOAL,
        FACTORY,
        FILTER,
        RECYCLER,
    }

    [System.Serializable]
    struct FileName
    {
        public FileFor fileFor;
        public string name;
    }

    [SerializeField]
    private FileName[] fileNames = new FileName[]
    {
        new FileName{ fileFor = FileFor.SCORE_WEIGHT, name = "ScoreWeight" },
        new FileName{ fileFor = FileFor.POLLUTANT, name = "Pollutant" },
        new FileName{ fileFor = FileFor.DISASTER, name = "Disaster" },
        new FileName{ fileFor = FileFor.COUNTRY, name = "Country" },
        new FileName{ fileFor = FileFor.GOAL, name = "Goal" },
        new FileName{ fileFor = FileFor.FACTORY, name = "Factory" },
        new FileName{ fileFor = FileFor.FILTER, name = "Filter" },
        new FileName{ fileFor = FileFor.RECYCLER, name = "Recycler" },
    };
    [SerializeField] private string dir = "Json/";
    [SerializeField] private string suffix = ".json";

    private AttribData attribData = null;

    public AttribData LoadLazy()
    {
        if (attribData == null)
        {
            var fileData = new Dictionary<FileFor, string> { };
            foreach (var filename in fileNames)
            {
#if UNITY_WEBGL
                var data = Resources.Load<TextAsset>(dir + filename.name);
#else
            var path = Application.dataPath + "/Resources/" + dir + filename.name + suffix;
            var data = System.IO.File.ReadAllText(path);
#endif
                // HACK(Xiaoyue Chen): replace "Phosphor" to "Emission"
                var data_str = data.ToString();
                var replaced_str = data_str.Replace("\"Phosphor\"", "\"Emission\"");
                fileData.Add(filename.fileFor, replaced_str);
            }

            var scoreWeight = JsonUtility.FromJson<ScoreWeight>(fileData[FileFor.SCORE_WEIGHT]);
            var pollutantList = JsonUtility.FromJson<PollutantList>(fileData[FileFor.POLLUTANT]);
            var disasterList = JsonUtility.FromJson<DisasterList>(fileData[FileFor.DISASTER]);
            var countryList = JsonUtility.FromJson<CountryList>(fileData[FileFor.COUNTRY]);
            var goalList = JsonUtility.FromJson<GoalList>(fileData[FileFor.GOAL]);
            var factoryList = LoadPolluterList<FactoryList>(fileData[FileFor.FACTORY]);
            var filterList = LoadPolluterList<FilterList>(fileData[FileFor.FILTER]);
            var recyclerList = LoadPolluterList<RecyclerList>(fileData[FileFor.RECYCLER]);

            attribData = new AttribData
            {
                scoreWeight = scoreWeight,
                pollutantList = pollutantList.pollutantList,
                disasterList = disasterList.disasterList,
                countryList = countryList.countryList,
                goalList = goalList.list,
                factoryList = factoryList.list,
                filterList = filterList.list,
                recyclerList = recyclerList.list
            };
        }

        return attribData;
    }

    private T LoadPolluterList<T>(string data) where T : IPolluterAttribList
    {
        var placementList = JsonUtility.FromJson<PlacementList>(data);
        var polluterList = JsonUtility.FromJson<T>(data);
        var baseList = polluterList.GetPolluterAttribs();
        for (int i = 0; i != baseList.Length; ++i)
        {
            var sig = placementList.list[i].placementAttrib.GetSignature();
            baseList[i].placementAttrib = new PlacementAttrib(sig);
        }
        return polluterList;
    }
}
