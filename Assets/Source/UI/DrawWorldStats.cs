using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWorldStats : MonoBehaviour
{
    WorldStateManager stateManager = null;
    Text text = null;

    void Start()
    {
        stateManager = WorldStateManager.FindWorldStateManager();
        text = GetComponent<Text>();
    }

    void Update()
    {
        // TODO(Xiaoyue Chen): using event system to update text
        float producedPollutionSum = stateManager.GetProducedPollutionSum();
        float filteredPollutionSum = stateManager.GetFilteredPollutionSum();
        float netPollutionSum = stateManager.GetNetPollutionSum();

        text.text =
            "World Stats: \n" +
            "Total produced pollution: " + producedPollutionSum.ToString() + "\n" +
            "Total filtered pollution: " + filteredPollutionSum.ToString() + "\n" +
            "Total pollution into the sea: " + netPollutionSum.ToString();

    }
}
