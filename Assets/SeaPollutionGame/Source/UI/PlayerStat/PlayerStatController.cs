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
    public Player GetPlayer() { return player; }

    [Header("Header")]
    [SerializeField]
    private TextMeshProUGUI txtCoinsTitle = null;
    [SerializeField]
    private TextMeshProUGUI txtCoinsValue = null;
    [SerializeField]
    private TextMeshProUGUI txtIncome = null;
    [SerializeField]
    private Color negativeIncomeColor = Color.red;
    private Color defaultColorTxt = Color.white;

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

        defaultColorTxt = txtCoinsValue.color;

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
        txtCoinsValue.text = Mathf.Round(value).ToString();
    }

    private void UpdateIncome(float value)
    {
        if(value == 0) { txtIncome.text = "0"; return; }

        string s = "";
        if(value > 0)
        {
            s += "+" + Mathf.Round(value);
            txtIncome.color = defaultColorTxt;
        } else
        {
            s += Mathf.Round(value);
            txtIncome.color = negativeIncomeColor;
        }
        s += "";

        txtIncome.text = s;
    }

    public void FeedbackCoins()
    {
        txtCoinsValue.DORestart();
        txtCoinsValue.DOKill();
        Sequence flashingSequenceValue = Feedback.Instance.ErrorText(txtCoinsValue, defaultColorTxt);

        /*
        txtCoinsTitle.DORestart();
        txtCoinsValue.DOKill();
        Sequence flashingSequenceTitle = Feedback.Instance.ErrorText(txtCoinsTitle, defaultColorTxt);

        flashingSequenceTitle.Play();
        */
        flashingSequenceValue.Play();
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
