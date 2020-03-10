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
        public int playerID = 0;
        public RectTransform playerInformation = null;
        public float tweenXOffset = 0f;
    }

    WorldStateManager worldStateManager = null;

    [SerializeField]
    private Button endTurnButton = null;
    [SerializeField]
    private Button newGameButton = null;

    [SerializeField]
    private List<InfoPlayerEndTurn> playersInfo = null;

    [SerializeField]
    private float tweenDuration = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private int currentPlayerID = 0;

    private void Start()
    {
        worldStateManager = FindObjectOfType<WorldStateManager>();
        if(worldStateManager == null) { Debug.LogError("[EndTurnController] Start: WorldStateManager not found"); return; }

        newGameButton.gameObject.SetActive(false);

        endTurnButton.onClick.AddListener(OnBtnClick);

        currentPlayerID = worldStateManager.GetCurrentPlayerID();
        
        ShowPlayer(currentPlayerID);

        worldStateManager.AddEndPlayerTurnFinishEventListener(OnEndTurn);
        worldStateManager.AddEndGameEventListener(OnEndGame);
    }

    private void OnDestroy()
    {
        endTurnButton.onClick.RemoveListener(OnBtnClick);
        newGameButton.onClick.RemoveListener(OnNewBtnClick);
    }

    private void OnEndTurn()
    {
        currentPlayerID = worldStateManager.GetCurrentPlayerID();

        HidePlayer(currentPlayerID);
        
        ShowPlayersExcept(currentPlayerID);
    }

    private void OnEndGame()
    {
        HidePlayersExcept();

        endTurnButton.gameObject.SetActive(false);

        newGameButton.gameObject.SetActive(true);
        newGameButton.onClick.AddListener(OnNewBtnClick);
    }

    private void OnBtnClick()
    {
        worldStateManager.EndPlayerTurn();
    }

    private void OnNewBtnClick()
    {
        UIManager.Instance.levelController.LoadRandomLevel();
    }

    private void ShowPlayer(int id)
    {
        InfoPlayerEndTurn currentPlayer = playersInfo.Find(x => x.playerID == id);

        currentPlayer.playerInformation.DOKill();
        currentPlayer.playerInformation.DOLocalMoveX(currentPlayer.tweenXOffset, tweenDuration).SetEase(tweenEase);
    }

    private void ShowPlayersExcept(int exceptFromId = 0)
    {
        foreach (InfoPlayerEndTurn ip in playersInfo)
        {
            if (ip.playerID != exceptFromId)
            {
                ShowPlayer(ip.playerID);
            }
        }
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

        currentPlayer.playerInformation.DOKill();
        currentPlayer.playerInformation.DOLocalMoveX(0, tweenDuration).SetEase(tweenEase);
    }
}
