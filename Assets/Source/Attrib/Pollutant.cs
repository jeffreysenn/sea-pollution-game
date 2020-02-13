[System.Serializable]
public class Pollutant
{
    public enum Type
    { }

    [System.NonSerialized] public Type type;
    public string title = "default pollution";
}
