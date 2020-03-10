using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnNumberController : MonoBehaviour
{
    WorldStateManager worldStateManager = null;

    [SerializeField]
    private TextMeshProUGUI txtNumber = null;
    [SerializeField]
    private TextMeshProUGUI txtTotal = null;

    private int currentNumberTurn = 1;
    private int totalNumberTurn = 12;

    private void Start()
    {
        worldStateManager = FindObjectOfType<WorldStateManager>();
        if (worldStateManager == null) { Debug.LogError("[TurnNumberController] Start: WorldStateManager not found"); return; }

        totalNumberTurn = worldStateManager.GetRemainingTurnCount();
        currentNumberTurn = 1;

        txtNumber.text = currentNumberTurn.ToString();
        txtTotal.text = totalNumberTurn.ToString();

        worldStateManager.AddEndTurnEventListener(OnEndTurn);
    }

    private void OnEndTurn()
    {
        if(totalNumberTurn != worldStateManager.GetRemainingTurnCount())
        {
            totalNumberTurn = worldStateManager.GetRemainingTurnCount();

            currentNumberTurn++;

            txtNumber.text = currentNumberTurn.ToString();
        }
    }
}
