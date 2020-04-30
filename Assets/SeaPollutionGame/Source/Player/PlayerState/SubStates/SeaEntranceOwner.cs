using System.Collections.Generic;

public class SeaEntranceOwner : ISeaEntranceOwner
{
    private List<SeaEntrance> seaEntrances = new List<SeaEntrance> { };

    public SeaEntrance[] GetSeaEntrances()
    {
        return seaEntrances.ToArray();
    }

    public void AddSeaEntrance(SeaEntrance seaEntrance)
    {
        seaEntrances.Add(seaEntrance);
    }
}
