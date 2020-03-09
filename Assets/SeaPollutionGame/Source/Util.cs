using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Util
{
    public static float SumMap(PollutionMap map)
    {
        float sum = 0;
        foreach (var pair in map)
        {
            sum += pair.Value;
        }
        return sum;
    }

    public static PollutionMap SumMap(Dictionary<Flow, PollutionMap> map)
    {
        var sum = new PollutionMap { };
        foreach (var pair in map)
        {
            sum += pair.Value;
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

    public static PollutionMap DivideMap(PollutionMap map, float denominator)
    {
        var result = new PollutionMap(map);
        foreach (var key in result.Keys.ToArray())
        {
            result[key] /= denominator;
        }
        return result;
    }

    public static void Shuffle<T>(IList<T> list)
    {
        Random rng = new Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
