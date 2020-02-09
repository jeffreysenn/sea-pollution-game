[System.Serializable]
public struct EconomicAttrib
{
    public float costToPurchase;
    public float profitPerTurn;

    public string GetDiscription()
    {
        return "Cost: " + costToPurchase.ToString("0.00") + "\n" +
               "Profit per turn: " + profitPerTurn.ToString("0.00") + "\n";
    }
}
