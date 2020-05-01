using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine.Events;

public class PlayerStateBlackboard
{
    public enum Key
    {
        MONEY,
        ASSET_VALUE,
        EFFICIENCY,
        SCORE,
        GOAL_BONUS,
        POLLUTION,
        RESOURCE,
    }

    private static Key kNonValueKeys = Key.POLLUTION | Key.RESOURCE;
    private Dictionary<Key, float> valueMap = new Dictionary<Key, float>();
    private Dictionary<PollutionMapType, PollutionMap> accumulatedPollutionMap = new Dictionary<PollutionMapType, PollutionMap> { };
    private ResourceMap accumulatedResourceMap = new ResourceMap { };

    public void Write(Key key, float val)
    {
        Debug.Assert((key & kNonValueKeys) != key);
        valueMap[key] = val;
    }

    public void Write(Key key, PollutionMapType pollutionMapType, string pollutionName, float val)
    {
        Debug.Assert(key == Key.POLLUTION);
        if (!accumulatedPollutionMap.ContainsKey(pollutionMapType))
        {
            accumulatedPollutionMap[pollutionMapType] = new PollutionMap { };
        }
        accumulatedPollutionMap[pollutionMapType][pollutionName] = val;
    }

    public void Write(Key key, string resourceName, float val)
    {
        Debug.Assert(key == Key.RESOURCE);
        accumulatedResourceMap[resourceName] = val;
    }

    public float GetValue(Key key)
    {
        Debug.Assert((key & kNonValueKeys) != key);
        return valueMap[key];
    }

    public float GetValue(Key key, PollutionMapType pollutionMapType, string pollutionName)
    {
        Debug.Assert(key == Key.POLLUTION);
        PollutionMap pollutionMap = null;
        var value = 0.0f;
        if (accumulatedPollutionMap.TryGetValue(pollutionMapType, out pollutionMap))
        {
            pollutionMap.TryGetValue(pollutionName, out value);
        }
        return value;
    }

    public float GetValue(Key key, string resourceName)
    {
        Debug.Assert(key == Key.RESOURCE);
        var value = 0.0f;
        accumulatedResourceMap.TryGetValue(resourceName, out value);
        return value;
    }

}