using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Util
{
    public static TValue SumMap<TKey, TValue>(Dictionary<TKey, TValue> map)
        where TValue : new()
    {
        dynamic sum = new TValue();
        foreach (var pair in map)
        {
            sum += (dynamic)pair.Value;
        }
        return sum;
    }

    public static PollutionMap MultiplyMap(PollutionMap map, float coefficient)
    {
        var result = new PollutionMap(map);
        foreach (var key in result.Keys.ToArray())
        {
            result[key] *= coefficient;
        }
        return result;
    }

    private PollutionMap DivideMap(PollutionMap map, float denominator)
    {
        var result = new PollutionMap(map);
        foreach (var key in result.Keys.ToList())
        {
            result[key] /= denominator;
        }
        return result;
    }
}
