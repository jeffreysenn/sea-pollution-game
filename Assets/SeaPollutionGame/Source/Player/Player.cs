using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int id = 0;

    void Awake()
    {
        var worldStateManager = FindObjectOfType<WorldStateManager>();
        worldStateManager.RegisterPlayer(id);

        var attribLoader = FindObjectOfType<AttribLoader>();
        var attribData = attribLoader.LoadLazy();
        var playerState = worldStateManager.GetPlayerState(id);
        playerState.SetScoreWeight(attribData.scoreWeight);
        playerState.SetGoals(attribData.goalList);
    }
}
