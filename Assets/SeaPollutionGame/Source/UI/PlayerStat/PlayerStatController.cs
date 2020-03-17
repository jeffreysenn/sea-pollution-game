using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatController : MonoBehaviour
{
    enum PlayerNumber { ONE, TWO };

    /*
     * TODO: Add Player Income data
     *       Add event when money of a player changes
     *       Add event when pollutionmap of a type changes
     *       Change "tweenYOffsetInitial" to get value from Start
     */



    WorldStateManager worldStateManager = null;

    [SerializeField]
    private PlayerNumber playerNumber = PlayerNumber.ONE;
    
    private Player player = null;

    [Header("Header")]
    [SerializeField]
    private TextMeshProUGUI txtCoinsValue = null;
    [SerializeField]
    private TextMeshProUGUI txtIncome = null;

    [Header("Content")]
    [SerializeField]
    private CanvasGroup contentCanvas = null;
    [SerializeField]
    private PlayerPieChart pieChart = null;

    [Header("Tween")]
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.InSine;
    
    private bool _isShown = true;
    public bool isShown { get { return _isShown; } }

    private void Start()
    {
        worldStateManager = UIManager.Instance.worldStateManager;

        if(playerNumber == PlayerNumber.ONE)
        {
            player = UIManager.Instance.player1;
        } else if (playerNumber == PlayerNumber.TWO)
        {
            player = UIManager.Instance.player2;
        }

        pieChart.SetPlayer(player);
        pieChart.Activate();

        Update();
    }

    private void Update()
    {
        var playerState = worldStateManager.GetPlayerState(player.id);
        UpdateCoins(playerState.GetMoney());
        UpdateIncome(playerState.GetTurnIncome());
    }
    
    private void UpdateCoins(float value)
    {
        txtCoinsValue.text = value.ToString();
    }

    private void UpdateIncome(float value)
    {
        if(value == 0) { txtIncome.text = ""; return; }

        string s = "(";
        if(value > 0)
        {
            s += "+" + value;
        } else
        {
            s += value;
        }
        s += ")";

        txtIncome.text = s;
    }

    public void Show()
    {
        if (_isShown) return;

        _isShown = true;

        contentCanvas.DOKill();
        contentCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
    }

    public void Hide()
    {
        if (!_isShown) return;

        _isShown = false;

        contentCanvas.DOKill();
        contentCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);
    }
}
