using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerStatController : MonoBehaviour
{
    /*
     * TODO: Add Player Income data
     *       Add event when money of a player changes
     *       Add event when pollutionmap of a type changes
     *       Change "tweenYOffsetInitial" to get value from Start
     */



    WorldStateManager worldStateManager = null;

    [SerializeField]
    private Player player = null;

    [Header("Header")]
    [SerializeField]
    private TextMeshProUGUI txtCoinsValue = null;
    [SerializeField]
    private TextMeshProUGUI txtIncome = null;

    [Header("Content")]
    [SerializeField]
    private PlayerPieChart pieChart = null;



    private bool isContentDetailsShown = false;

    private void Awake()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if (worldStateManager == null) { Debug.LogError("[PlayerStatController] Start: WorldStateManager not found"); return; }
    }

    private void Start()
    {
        pieChart.SetPlayer(player);
        pieChart.Activate();

        UpdateCoins(worldStateManager.GetMoney(player.id));
        UpdateIncome(worldStateManager.GetIncome(player.id));
    }

    private void Update()
    {
        UpdateCoins(worldStateManager.GetMoney(player.id));
        UpdateIncome(worldStateManager.GetIncome(player.id));
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
}
