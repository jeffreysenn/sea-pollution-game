[System.Serializable]
public class BaseEmissionAttrib
{
    public PollutionAttrib urban, agriculture, forest, ocean;
}

[System.Serializable]
public class CountryAttrib
{
    public string countryName;
    public BaseEmissionAttrib baseEmission;
}
