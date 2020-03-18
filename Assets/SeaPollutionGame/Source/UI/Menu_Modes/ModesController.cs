using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ModesController : MonoBehaviour
{
    [SerializeField]
    private ModeToggle toggleTutorial = null;
    [SerializeField]
    private ModeToggle toggleFlows = null;
    
    [SerializeField]
    private TutorialController tutorialController = null;

    private FlowManager flowManager = null;

    private void Start()
    {
        flowManager = UIManager.Instance.flowManager;
        flowManager.OnDisplay += FlowManager_OnDisplay;

        toggleTutorial.OnToggle += ToggleTutorial_OnToggle;
        toggleFlows.OnToggle += ToggleFlows_OnToggle;

        toggleTutorial.toggled = false;
        toggleFlows.toggled = false;
    }

    private void FlowManager_OnDisplay(bool shown)
    {
        if(!shown)
        {
            if (toggleFlows.toggled)
            {
                flowManager.Show();
            }
        }
    }

    private void OnDestroy()
    {
        toggleTutorial.OnToggle -= ToggleTutorial_OnToggle;
        toggleFlows.OnToggle -= ToggleFlows_OnToggle;
    }

    private void ToggleTutorial_OnToggle(ToggleButton btn, bool toggle)
    {
        if (toggle)
            tutorialController.Show();
        else
            tutorialController.Hide();
    }
    
    private void ToggleFlows_OnToggle(ToggleButton btn, bool toggle)
    {
        if (toggle)
            flowManager.Show();
        else
            flowManager.Hide();
    }



}
