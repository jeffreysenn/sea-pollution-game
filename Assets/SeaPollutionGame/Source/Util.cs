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
}
