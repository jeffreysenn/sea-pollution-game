using System;

[System.Serializable]
public struct PollutionAttrib
{
    public Pollution pollution;
    public float emissionPerTurn;

    public string GetDiscription()
    {
        string[] preds = { "Cause", "Reduce" };
        var pred = preds[Convert.ToInt32(emissionPerTurn < 0)];
        return pred + ": " + pollution.pollutionName + "\n" 
            + pred + " pollution per turn: " + emissionPerTurn.ToString("0.00") + "\n";
    }
}
