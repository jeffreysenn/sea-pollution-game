using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int id = 0;

    void Start()
    {
        var worldStateObj = FindObjectOfType<WorldStateManager>();
        var worldState = worldStateObj.GetComponent<WorldStateManager>();
        worldState.RegisterPlayer(id);
    }
}
