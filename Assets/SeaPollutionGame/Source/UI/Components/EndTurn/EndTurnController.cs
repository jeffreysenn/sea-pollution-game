using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class EndTurnController : MonoBehaviour
{
    /*
     * TODO: event when turn starts (callback will be what is in Update)
     */

    [System.Serializable]
    class InfoPlayerEndTurn
    {
        public int playerID;
        public RectTransform playerInformation;
        public float tweenXOffset;
    }

    WorldStateManager worldStateManager = null;

    [SerializeField]
    private Button endTurnButton = null;

    [SerializeField]
    private List<InfoPlayerEndTurn> playersInfo;

    [SerializeField]
    private float tweenDuration = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private int currentPlayerID = 0;

    private void Start()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if(worldStateManager == null) { Debug.LogError("[EndTurnController] Start: WorldStateManager not found"); return; }

        endTurnButton.onClick.AddListener(OnBtnClick);

        currentPlayerID = worldStateManager.GetCurrentPlayerID();
        
        ShowPlayer(currentPlayerID);

        worldStateManager.AddEndPlayerTurnFinishEventListener(OnEndTurn);
    }

    private void OnDestroy()
    {
        endTurnButton.onClick.RemoveListener(OnBtnClick);
    }

    private void Update()
    {
        if (currentPlayerID != worldStateManager.GetCurrentPlayerID())
        {
            currentPlayerID = worldStateManager.GetCurrentPlayerID();
            ShowPlayer(currentPlayerID);
        }
    }

    private void OnEndTurn()
    {
        currentPlayerID = worldStateManager.GetCurrentPlayerID();

        HidePlayer(currentPlayerID);
    }

    private void OnBtnClick()
    {
        worldStateManager.EndPlayerTurn();
    }

    private void ShowPlayer(int id)
    {
        InfoPlayerEndTurn currentPlayer = playersInfo.Find(x => x.playerID == id);
        currentPlayer.playerInformation.DOLocalMoveX(currentPlayer.tweenXOffset, tweenDuration).SetEase(tweenEase);
    }

    private void HidePlayersExcept(int exceptFromId = 0)
    {
        foreach(InfoPlayerEndTurn ip in playersInfo)
        {
            if(ip.playerID != exceptFromId)
            {
                HidePlayer(ip.playerID);
            }
        }
    }

    private void HidePlayer(int id)
    {
        InfoPlayerEndTurn currentPlayer = playersInfo.Find(x => x.playerID == id);
        currentPlayer.playerInformation.DOLocalMoveX(0, tweenDuration).SetEase(tweenEase);
    }
}
