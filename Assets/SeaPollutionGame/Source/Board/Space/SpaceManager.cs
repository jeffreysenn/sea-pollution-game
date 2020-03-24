using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    private Space[] _spaces;
    public Space[] spaces { get { return _spaces; } }

    private bool isHighlighting = false;

    private void Awake()
    {
        _spaces = FindObjectsOfType<Space>();
    }

    public void HightlightAvailablePlaces(int playerID, PolluterAttrib polluterAttrib)
    {
        if(isHighlighting)
        {
            HideHighlight();
        }

        isHighlighting = true;

        foreach (Space s in spaces)
        {
            if (s.CanPlacePolluter(playerID, polluterAttrib))
            {
                s.Highlight();
            }
        }
    }

    public void HideHighlight()
    {
        if (!isHighlighting) return;

        foreach (Space s in spaces)
        {
            s.HideHighlight();
        }

        isHighlighting = false;
    }
}
