using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : Polluter
{
    public override void Mulfunction()
    {
        base.Mulfunction();
        SetProfit(0);
    }

    public override void Operate(PollutionMap input)
    {
        base.Operate(input);
        var resourceMap = GetResourceMap();
        resourceMap.Clear();
        var recycleAttrib = GetAttrib().recycleAttrib;
        if (recycleAttrib.conversions != null)
        {
            foreach (var conversion in recycleAttrib.conversions)
            {
                if (input.ContainsKey(conversion.pollutantName))
                {
                    float pollution = input[conversion.pollutantName];
                    float maxConversion = conversion.maxConversion;
                    float proccessed = pollution > maxConversion ? maxConversion : pollution;
                    float converted = proccessed * conversion.conversionRate;
                    if (conversion.convertTo == "Money") { SetProfit(GetAttrib().economicAttrib.profitPerTurn + converted); }
                    else
                    {
                        resourceMap.Add(conversion.convertTo, converted);
                    }
                }
            }
        }
    }
}
