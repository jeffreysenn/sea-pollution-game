using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : Node
{
    public int ownerID = -1;
    [SerializeField] private GameObject highlightPrefab = null;
    private Polluter polluter = null;

    private GameObject highlight = null;

    public virtual bool CanPlacePolluter(int playerID, Polluter polluter)
    {
        var placementAttrib = polluter.GetAttrib().placementAttrib;
        return placementAttrib.CanPlaceOn(GetPlaceType()) && polluter == null && ownerID == playerID;
    }

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

    public void Highlight()
    {
        if (!highlight)
        {
            highlight = Instantiate(highlightPrefab, transform);
        }

        highlight.SetActive(true);
    }

    public void HideHighlight()
    {
        if (highlight) { highlight.SetActive(false); }
    }

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