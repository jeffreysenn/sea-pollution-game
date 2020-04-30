using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

[System.Serializable]
public class CameraRef
{
    public enum Name
    {
        GAME,
        CINEMA,
    }
    public Name name;
    public CinemachineVirtualCamera cam;
    public CinemachineDollyCart cart;
}

public class CameraManager : MonoBehaviour
{
    public enum State
    {
        GAME,
        CINEMA,
    }

    [SerializeField] private float trackEnd = 0.8f;
    private State state = State.GAME;
    private Dictionary<State, CinemachineVirtualCamera> camMap = new Dictionary<State, CinemachineVirtualCamera> { };
    private CinemachineVirtualCamera gameCam = null, cineCam = null;
    private CinemachineDollyCart dolly = null;

    public event Action<State> OnStateChanged;

    public void SetState(State state)
    {
        if (state == State.CINEMA)
        {
            dolly.m_Position = 0;
        }
        SwitchCamera();
        this.state = state;
        OnStateChanged?.Invoke(state);
    }

    private void Awake()
    {
        var vcams = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var cam in vcams)
        {
            if (cam.enabled == true)
            {
                camMap[State.GAME] = cam;
            }
            else
            {
                camMap[State.CINEMA] = cam;
            }
        }
        dolly = FindObjectOfType<CinemachineDollyCart>();
    }

    private void SwitchCamera()
    {
        foreach (var pair in camMap)
        {
            pair.Value.enabled = !pair.Value.enabled;
        }
    }

    private void Update()
    {
        if (state == State.CINEMA)
        {
            if (dolly.m_Position >= trackEnd)
            {
                SetState(State.GAME);
            }
        }
    }

}
