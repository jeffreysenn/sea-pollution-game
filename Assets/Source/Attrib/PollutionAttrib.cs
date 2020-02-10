using System;

[System.Serializable]
public struct PollutionAttrib
{
    public Pollution pollution;
    public float emissionPerTurn;

    public string GetDiscription()
    {
        string[] preds = { "Cause", "Reduce" };
        float[] dir = { 1, -1 };
        int negativeEmission = Convert.ToInt32(emissionPerTurn < 0);
        var pred = preds[negativeEmission];
        return pred + ": " + pollution.pollutionName + "\n" 
            + pred + " pollution per turn: " + (dir[negativeEmission] * emissionPerTurn).ToString("0.00") + "\n";
    }
}
