[System.Serializable]
public class VisualAttrib : System.ICloneable
{
    public string imageName = "default";

    public object Clone()
    {
        return MemberwiseClone();
    }

    public string GetDiscription()
    {
        return "ImageName: " + imageName + "\n";
    }
}