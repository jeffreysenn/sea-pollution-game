using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySpace : Space {
    public override bool CanPlacePolluter(int playerID, Polluter polluter)
    {
        if(base.CanPlacePolluter(playerID, polluter))
        {
            return polluter.GetComponent<Factory>();
        }
        return false;
    }
}