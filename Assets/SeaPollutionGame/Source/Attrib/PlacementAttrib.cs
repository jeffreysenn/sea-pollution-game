using System.Collections;
using System.Collections.Generic;

public enum PlaceType
{
    NONE = 0,
    OCEAN = 1 << 0,
    URBAN = 1 << 1,
    AGRICULTURE = 1 << 2,
    FOREST = 1 << 3,
}

public class PlacementAttrib : System.ICloneable
{
    private PlaceType signature = PlaceType.OCEAN | PlaceType.URBAN | PlaceType.AGRICULTURE | PlaceType.FOREST;

    public static PlaceType MakeSignature(PlaceType[] supportPlaces)
    {
        var result = PlaceType.NONE;
        foreach(var type in supportPlaces)
        {
            result |= type;
        }
        return result;
    }

    public PlacementAttrib() { }
    public PlacementAttrib(PlaceType sig) { signature = sig; }

    public object Clone() { return MemberwiseClone(); }

    public bool CanPlaceOn(PlaceType type) { return (signature & type) == type; }
    public PlaceType GetSignature() { return signature; }
}
