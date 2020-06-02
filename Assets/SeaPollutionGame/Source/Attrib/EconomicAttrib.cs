[System.Serializable]
public class EconomicAttrib : System.ICloneable
{
    public float price;
    public float profitPerTurn;
    public float removalCost = 10;
    public float repairExtraRatio = 0.1f;

    public object Clone()
    {
        return MemberwiseClone();
    }

    public string GetDiscription()
    {
        return "Cost: " + price.ToString() + "\n" +
               "Profit per turn: " + profitPerTurn.ToString() + "\n";
    }
}
