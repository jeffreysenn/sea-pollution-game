using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayersStatController : MonoBehaviour
{
    [SerializeField]
    private PlayerStatController playerAController = null;
    [SerializeField]
    private PlayerStatController playerBController = null;
    [SerializeField]
    private ScoreMenu scoreMenu = null;
    [SerializeField]
    private GoalsMenu goalsMenu = null;

    private void Start()
    {
        scoreMenu.OnClick += ScoreMenu_OnClick;
        goalsMenu.OnClick += GoalsMenu_OnClick;
    }

    public void ToggleGoalsMenu()
    {
        if (goalsMenu.isShown)
        {
            playerAController.Show();
            playerBController.Show();
            scoreMenu.Hide();
            goalsMenu.Hide();
        }
        else
        {
            playerAController.Hide();
            playerBController.Hide();
            scoreMenu.Hide();
            goalsMenu.Show();
        }
    }

    public void ToggleScoresMenu()
    {
        if (scoreMenu.isShown)
        {
            playerAController.Show();
            playerBController.Show();
            goalsMenu.Hide();
            scoreMenu.Hide();
        }
        else
        {
            playerAController.Hide();
            playerBController.Hide();
            goalsMenu.Hide();
            scoreMenu.Show();
        }
    }

    private void GoalsMenu_OnClick(GoalsMenu menu)
    {
        ToggleGoalsMenu();
    }

    private void ScoreMenu_OnClick(ScoreMenu menu)
    {
        ToggleScoresMenu();
    }

    public void FeedbackNotEnoughCoins(int playerId)
    {
        if(playerId == playerAController.GetPlayer().id)
        {
            playerAController.FeedbackCoins();
        }

        if(playerId == playerBController.GetPlayer().id)
        {
            playerBController.FeedbackCoins();
        }
    }

    public ScoreMenu GetScoreMenu() { return scoreMenu; }
}
