[System.Serializable]
public struct EconomicAttrib
{
    public float price;
    public float profitPerTurn;

    public string GetDiscription()
    {
        return "Cost: " + price.ToString("0.00") + "\n" +
               "Profit per turn: " + profitPerTurn.ToString("0.00") + "\n";
    }
}
