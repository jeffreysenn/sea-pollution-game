using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    void Start()
    {
        var worldStateObj = FindObjectOfType<WorldStateManager>();
        var worldState = worldStateObj.GetComponent<WorldStateManager>();
        var button = GetComponent<Button>();
        button.onClick.AddListener(worldState.EndPlayerTurn);
    }

}
