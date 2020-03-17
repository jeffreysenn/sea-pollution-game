using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySpace : Space {
    public override bool CanPlacePolluter(int playerID, PolluterAttrib attrib)
    {
        if(base.CanPlacePolluter(playerID, attrib))
        {
            return (attrib is FactoryAttrib) || (attrib is RecyclerAttrib);
        }
        return false;
    }
}