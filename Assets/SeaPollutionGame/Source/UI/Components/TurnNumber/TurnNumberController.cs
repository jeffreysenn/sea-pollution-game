using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnNumberController : MonoBehaviour
{
    /*
     * TODO: Add an event in WorldStateManager that broadcast "end of turn" and/or when turn count changes
     *      => removing Update() check
     */

    WorldStateManager worldStateManager = null;

    [SerializeField]
    private TextMeshProUGUI txtNumber = null;
    [SerializeField]
    private TextMeshProUGUI txtTotal = null;

    private int currentNumberTurn = 1;
    private int totalNumberTurn = 12;

    private void Start()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if (worldStateManager == null) { Debug.LogError("[TurnNumberController] Start: WorldStateManager not found"); return; }

        totalNumberTurn = worldStateManager.GetRemainingTurnCount();
        currentNumberTurn = 1;

        txtNumber.text = currentNumberTurn.ToString();
        txtTotal.text = totalNumberTurn.ToString();
    }

    private void Update()
    {
        if(totalNumberTurn != worldStateManager.GetRemainingTurnCount())
        {
            totalNumberTurn = worldStateManager.GetRemainingTurnCount();

            currentNumberTurn++;

            txtNumber.text = currentNumberTurn.ToString();
        }
    }
}
