using System.Collections;
using System.Collections.Generic;

public class PlayerState
{
    public float money = 100;
    public float producedPollution = 0;
    public float netPollution = 0;
    public float GetNetPollution() { return netPollution; }
    public float GetFilteredPollution() { return producedPollution - netPollution; }
    public float GetProducedPollution() { return producedPollution; }
}