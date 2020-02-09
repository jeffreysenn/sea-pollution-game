using System;

[System.Serializable]
public struct PollutionAttrib
{
    public Pollution pollution;
    public float emissionPerTurn;

    public string GetDiscription()
    {
        string[] preds = { "Cause", "Reduce" };
        return preds[Convert.ToInt32(emissionPerTurn < 0)] + ": " + pollution.pollutionName + "\n" 
            + "by" + emissionPerTurn.ToString("0.00") + "\n";
    }
}
