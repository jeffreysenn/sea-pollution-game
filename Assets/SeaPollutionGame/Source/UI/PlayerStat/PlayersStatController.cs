using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void GoalsMenu_OnClick(GoalsMenu menu)
    {
        if(menu.isShown)
        {
            playerAController.Show();
            playerBController.Show();
            scoreMenu.Hide();
            menu.Hide();
        } else
        {
            playerAController.Hide();
            playerBController.Hide();
            scoreMenu.Hide();
            menu.Show();
        }
    }

    private void ScoreMenu_OnClick(ScoreMenu menu)
    {
        if(menu.isShown)
        {
            playerAController.Show();
            playerBController.Show();
            goalsMenu.Hide();
            menu.Hide();
        } else
        {
            playerAController.Hide();
            playerBController.Hide();
            goalsMenu.Hide();
            menu.Show();
        }
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
}
