using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterSpace : Space {
    public override bool CanPlacePolluter(int playerID, Polluter polluter)
    {
        if(base.CanPlacePolluter(playerID, polluter))
        {
            return polluter.GetComponent<Filter>();
        }
        return false;
    }
}
