using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : Node
{
    public int ownerID = -1;
    public Polluter polluter;

    public PlaceType GetPlaceType()
    {
        var parentBoard = GetComponentInParent<BoardPiece>();
        if (parentBoard)
        {
            return parentBoard.GetPlaceType();
        }
        return PlaceType.NONE;
    }

    public bool HasOwner() { return ownerID > 0 || polluter; }
}