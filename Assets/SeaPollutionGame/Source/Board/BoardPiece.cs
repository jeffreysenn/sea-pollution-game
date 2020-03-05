using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour
{
    [SerializeField] private PlaceType type = PlaceType.NONE;

    public PlaceType GetPlaceType() { return type; }
}
