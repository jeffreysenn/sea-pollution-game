using System.Collections;
using System.Collections.Generic;

public class PlayerState
{
    public float money = 100;
    public float producedPollution = 0;
    public float filteredPollution = 0;
    public float GetNetPollution() { return producedPollution - filteredPollution; }
}