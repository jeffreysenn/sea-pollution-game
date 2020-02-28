using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaEntrance : Node
{
    public int ownerID = -1;
    PollutionMap pollutionMap = new PollutionMap { };
    public float warningThreashold = 8;
    public Color fromColor = Color.cyan;
    public Color toColor = Color.red;

    public void SetPollutionMap(PollutionMap pollution)
    {
        pollutionMap = pollution;
        var drawDescrip = GetComponent<DrawDescription>();
        drawDescrip.SetPollutionMap(pollutionMap);
        foreach (Transform child in transform)
        {
            var spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                var targetColor = Color.Lerp(fromColor, toColor, pollutionMap.GetTotalPollution() / warningThreashold);
                spriteRenderer.color = targetColor;
            }
        }
    }

    void ReportPollution()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddPollution(ownerID, PollutionMapType.NET, pollutionMap);
    }

    public override void Start()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddEndPlayerTurnEventListener(ownerID, ReportPollution);
    }
}
