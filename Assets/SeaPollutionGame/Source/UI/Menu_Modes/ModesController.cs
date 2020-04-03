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
    private ModeToggle toggleGoals = null;
    [SerializeField]
    private ModeToggle toggleScores = null;
    
    [SerializeField]
    private TutorialController tutorialController = null;
    [SerializeField]
    private PlayersStatController playersStatController = null;

    private FlowManager flowManager = null;
    private ScoreMenu scoreMenu = null;
    private CustomBarChart scoreMainBar = null;

    private void Start()
    {
        flowManager = UIManager.Instance.flowManager;
        flowManager.OnDisplay += FlowManager_OnDisplay;

        scoreMenu = playersStatController.GetScoreMenu();
        scoreMainBar = scoreMenu.GetScoreBar();

        toggleTutorial.OnToggle += ToggleTutorial_OnToggle;
        toggleFlows.OnToggle += ToggleFlows_OnToggle;
        toggleGoals.OnToggle += ToggleGoals_OnToggle;
        toggleScores.OnToggle += ToggleScores_OnToggle;

        toggleTutorial.toggled = false;
        toggleFlows.toggled = false;
        toggleGoals.toggled = false;
        toggleScores.toggled = false;

        scoreMainBar.OnValueChanged += ScoreMainBar_OnValueChanged;
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

    private void ToggleGoals_OnToggle(ToggleButton btn, bool toggle)
    {
        playersStatController.ToggleGoalsMenu();
    }

    private void ToggleScores_OnToggle(ToggleButton btn, bool toggle)
    {
        playersStatController.ToggleScoresMenu();
    }
    
    private void ScoreMainBar_OnValueChanged(CustomBarChart obj)
    {
        toggleScores.transform.position = obj.GetInBetweenPosition();
    }

}
