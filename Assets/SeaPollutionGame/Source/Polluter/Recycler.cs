using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : Polluter
{
    public override void Mulfunction()
    {
        base.Mulfunction();
        var economicAttrib = GetAttrib().economicAttrib;
        SetProfit(economicAttrib.profitPerTurn);
        GetPollutionMap().Clear();
    }

    public override void Operate(PollutionMap input)
    {
        if (!GetHealthComp().IsAlive()) { return; }

        base.Operate(input);

        GetPollutionMap().Clear();
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
                    var pollutionMap = GetPollutionMap();
                    if (!pollutionMap.ContainsKey(conversion.pollutantName)) { pollutionMap.Add(conversion.pollutantName, 0); }
                    pollutionMap[conversion.pollutantName] -= proccessed;
                    float converted = proccessed * conversion.conversionRate;
                    if (conversion.convertTo == "Money") { SetProfit(GetAttrib().economicAttrib.profitPerTurn + converted); }
                    else
                    {
                        GetResourceMap().Add(conversion.convertTo, converted);
                    }
                }
            }
        }
    }
}
