using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuInGameController : MonoBehaviour
{
    [SerializeField]
    private LevelController levelController = null;

    [SerializeField]
    private Button btnOpen = null;
    [SerializeField]
    private Button btnRestart = null;
    [SerializeField]
    private Button btnQuit = null;

    [SerializeField]
    private CanvasGroup menuContent = null;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private bool isShown = false;

    private void Start()
    {
        HideDirectMenu();

        btnOpen.onClick.AddListener(OpenOnClick);
        btnRestart.onClick.AddListener(RestartOnClick);
        btnQuit.onClick.AddListener(QuitOnClick);
    }

    private void OnDestroy()
    {
        btnOpen.onClick.RemoveListener(OpenOnClick);
        btnRestart.onClick.RemoveListener(RestartOnClick);
        btnQuit.onClick.RemoveListener(QuitOnClick);
    }

    void OpenOnClick()
    {
        if(!isShown)
        {
            ShowMenu();
        } else
        {
            HideMenu();
        }
    }

    void RestartOnClick()
    {
        levelController.LoadRandomLevel();
    }

    void QuitOnClick()
    {
        // return to home menu
    }

    void ShowMenu()
    {
        menuContent.DOFade(1f, tweenDuration).SetEase(tweenEase);

        isShown = true;
    }

    void HideMenu()
    {
        menuContent.DOFade(0f, tweenDuration).SetEase(tweenEase);

        isShown = false;
    }

    void HideDirectMenu()
    {
        menuContent.DOFade(0f, 0f);

        isShown = false;
    }
}
