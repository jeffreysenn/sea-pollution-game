[System.Serializable]
public struct EconomicAttrib
{
    public float price;
    public float profitPerTurn;

    public string GetDiscription()
    {
        return "Cost: " + price.ToString() + "\n" +
               "Profit per turn: " + profitPerTurn.ToString() + "\n";
    }
}
