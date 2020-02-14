using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaEntrance : MonoBehaviour
{
    public int ownerID = -1;
    PollutionMap pollutionMap = new PollutionMap { };
    public float warningThreashold = 5;
    public Color fromColor = Color.cyan;
    public Color toColor = Color.red;

    public void SetPollutionMap(PollutionMap pollution)
    {
        pollutionMap = pollution;
        var drawDescrip = GetComponent<DrawDescription>();
        drawDescrip.SetDescription("Pollution into the sea(per turn):\n" + pollutionMap.GetDescription());
        foreach(Transform child in transform)
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
        stateManager.AddNetPollution(ownerID, pollutionMap);
    }

    void Start()
    {
        var stateManager = FindObjectOfType<WorldStateManager>().GetComponent<WorldStateManager>();
        stateManager.AddEndPlayerTurnEventListener(ownerID, ReportPollution);
    }
}
