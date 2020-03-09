using UnityEngine;

public class Factory : Polluter
{
    public override void Mulfunction()
    {
        base.Mulfunction();
        SetProfit(0);
    }
}
