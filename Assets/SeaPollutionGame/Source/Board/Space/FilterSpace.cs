using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterSpace : Space
{
    public override bool CanPlacePolluter(int playerID, PolluterAttrib attrib)
    {
        if (base.CanPlacePolluter(playerID, attrib))
        {
            return (attrib is FilterAttrib) || (attrib is RecyclerAttrib);
        }
        return false;
    }
}
