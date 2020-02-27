using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class EndTurnController : MonoBehaviour
{
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
    private float tweenAnimationSpeed = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private void Start()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
        if(worldStateManager == null) { Debug.LogError("[EndTurnController] Start: WorldStateManager not found"); return; }

        endTurnButton.onClick.AddListener(OnBtnClick);
        
        ShowPlayer(worldStateManager.GetCurrentPlayerID());
    }

    private void OnDestroy()
    {
        endTurnButton.onClick.RemoveListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        worldStateManager.EndPlayerTurn();

        int id = worldStateManager.GetCurrentPlayerID();
        HidePlayers(id);
        ShowPlayer(id);
    }

    private void ShowPlayer(int id)
    {
        InfoPlayerEndTurn currentPlayer = playersInfo.Find(x => x.playerID == id);
        currentPlayer.playerInformation.DOLocalMoveX(currentPlayer.tweenXOffset, tweenAnimationSpeed).SetEase(tweenEase);
    }

    private void HidePlayers(int exceptFromId = 0)
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
        currentPlayer.playerInformation.DOLocalMoveX(0, tweenAnimationSpeed).SetEase(tweenEase);
    }
}
