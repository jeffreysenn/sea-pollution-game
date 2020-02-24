[System.Serializable]
public class EconomicAttrib
{
    public float price;
    public float profitPerTurn;
    public float removalCost = 10;

    public string GetDiscription()
    {
        return "Cost: " + price.ToString() + "\n" +
               "Profit per turn: " + profitPerTurn.ToString() + "\n";
    }
}
