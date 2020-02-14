using System;

[System.Serializable]
public struct PollutionAttrib
{
    [System.Serializable]
    public struct Emission
    {
        public string pollutantName;
        public float emissionPerTurn;
    }

    public Emission[] emissions;

    public string GetDiscription()
    {
        string cause = "";
        string reduce = "";
        foreach (var emission in emissions)
        {
            if (emission.emissionPerTurn > 0)
            {
                cause += (emission.pollutantName + ": " + emission.emissionPerTurn.ToString("0.00") + "\n");
            }
            else if (emission.emissionPerTurn < 0)
            {
                reduce += (emission.pollutantName + ": " + (-emission.emissionPerTurn).ToString("0.00") + "\n");
            }
        }
        string result = "";
        if(cause.Length > 0)
        {
            result += ("Cause pollution:\n" + cause);
        }
        if(reduce.Length > 0)
        {
            result += ("Reduce pollution:\n" + reduce);
        }
        return result;
    }
}
