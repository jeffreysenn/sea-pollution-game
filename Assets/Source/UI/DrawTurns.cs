using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawTurns : MonoBehaviour
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
        int remainingTurnCount = stateManager.GetRemainingTurnCount();
        int currentPlayerID = stateManager.GetCurrentPlayerID();
        text.text = remainingTurnCount.ToString() + " TURNS LEFT\n"
            + "Player " + currentPlayerID.ToString() + "'s turn";
    }
}
