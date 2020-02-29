[System.Serializable]
public struct PollutionAttrib : System.ICloneable
{
    [System.Serializable]
    public struct Emission : System.ICloneable
    {
        public string pollutantName;
        public float emissionPerTurn;

        public object Clone()
        {
            var clone = (Emission)MemberwiseClone();
            clone.pollutantName = (string)pollutantName.Clone();
            return this;
        }
    }

    public Emission[] emissions;

    public object Clone()
    {
        var clone = new PollutionAttrib { };
        clone.emissions = new Emission[emissions.Length];
        for (int i = 0; i != clone.emissions.Length; ++i)
        {
            clone.emissions[i] = (Emission)emissions[i].Clone();
        }
        return clone;
    }

    public string GetDiscription()
    {
        if (emissions == null) { return ""; }
        string cause = "";
        string reduce = "";
        foreach (var emission in emissions)
        {
            if (emission.emissionPerTurn > 0)
            {
                cause += (emission.pollutantName + ": " + emission.emissionPerTurn.ToString() + "\n");
            }
            else if (emission.emissionPerTurn < 0)
            {
                reduce += (emission.pollutantName + ": " + (-emission.emissionPerTurn).ToString() + "\n");
            }
        }
        string result = "";
        if (cause.Length > 0)
        {
            result += ("Cause pollution:\n" + cause);
        }
        if (reduce.Length > 0)
        {
            result += ("Reduce pollution:\n" + reduce);
        }
        return result;
    }
}
