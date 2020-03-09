using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : Node
{
    public int ownerID = -1;
    private Polluter polluter = null;

    public void SetPolluter(Polluter polluter)
    {
        this.polluter = polluter;
        OperatePolluter();
    }

    public Polluter GetPolluter() { return polluter; }

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

    private void Start()
    {
        var inputEvent = GetInputEvent();
        inputEvent.AddListener((Flow, PollutionMap) => OperatePolluter());
    }

    private void OperatePolluter()
    {
        if (polluter)
        {
            polluter.Operate(GetInputPollutionMap());
            SetLocalPollution(polluter.GetPollutionMap());
        }
        else
        {
            SetLocalPollution(new PollutionMap { });
        }
    }
}