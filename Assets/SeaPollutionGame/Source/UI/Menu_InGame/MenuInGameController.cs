using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuInGameController : MonoBehaviour
{
    private SingletonLevelManager levelController = null;

    [Header("Buttons")]
    [SerializeField]
    private Button btnOpen = null;
    [SerializeField]
    private Button btnRestart = null;
    [SerializeField]
    private Button btnNewGame = null;
    [SerializeField]
    private Button btnQuit = null;
    [SerializeField]
    private float timeBeforeAction = 1f;

    [Header("Settings")]
    [SerializeField]
    private Toggle toggleTransition = null;
    [SerializeField]
    private Slider sliderVolume = null;
    private DisasterManager disasterManager = null;

    [Header("Tween")]
    [SerializeField]
    private CanvasGroup menuContent = null;
    private RectTransform menuTransform = null;
    [SerializeField]
    private Vector2 menuTargetPosition = Vector2.zero;
    private Vector2 menuDefaultPosition = Vector2.zero;

    [SerializeField]
    private CanvasGroup settingsContent = null;
    private RectTransform settingsTransform = null;
    [SerializeField]
    private Vector2 settingsTargetPosition = Vector2.zero;
    private Vector2 settingsDefaultPosition = Vector2.zero;

    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip openMenu = null;
    [SerializeField]
    private AudioClip closeMenu = null;

    private bool isShown = false;

    private void Start()
    {
        levelController = UIManager.Instance.levelController;
        disasterManager = UIManager.Instance.disasterManager;

        menuTransform = menuContent.GetComponent<RectTransform>();
        menuDefaultPosition = menuTransform.anchoredPosition;

        settingsTransform = settingsContent.GetComponent<RectTransform>();
        settingsDefaultPosition = settingsTransform.anchoredPosition;

        toggleTransition.isOn = disasterManager.transitionEnabled;
        sliderVolume.value = UIManager.Instance.GetVolume();

        HideDirectMenu();

        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    void AddListeners()
    {
        btnOpen.onClick.AddListener(OpenOnClick);
        btnRestart.onClick.AddListener(RestartOnClick);
        btnNewGame.onClick.AddListener(NewGameOnClick);
        btnQuit.onClick.AddListener(QuitOnClick);

        toggleTransition.onValueChanged.AddListener(TransitionOnValueChanged);
        sliderVolume.onValueChanged.AddListener(VolumeValueChanged);
    }

    void RemoveListeners()
    {
        btnOpen.onClick.RemoveListener(OpenOnClick);
        btnRestart.onClick.RemoveListener(RestartOnClick);
        btnNewGame.onClick.RemoveListener(NewGameOnClick);
        btnQuit.onClick.RemoveListener(QuitOnClick);

        toggleTransition.onValueChanged.RemoveListener(TransitionOnValueChanged);
        sliderVolume.onValueChanged.RemoveListener(VolumeValueChanged);
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
        RemoveListeners();
        StartCoroutine(RestartTimer());
    }

    void NewGameOnClick()
    {
        RemoveListeners();
        StartCoroutine(NewGameTimer());
    }

    void QuitOnClick()
    {
        RemoveListeners();
        StartCoroutine(QuitTimer());
    }

    void TransitionOnValueChanged(bool v)
    {
        disasterManager.transitionEnabled = v;
    }

    void VolumeValueChanged(float v)
    {
        UIManager.Instance.ChangeVolume(v);
    }

    IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(timeBeforeAction);
        levelController.LoadCurrentLevel();
    }

    IEnumerator NewGameTimer()
    {
        yield return new WaitForSeconds(timeBeforeAction);
        levelController.LoadRandomLevel();
    }

    IEnumerator QuitTimer()
    {
        yield return new WaitForSeconds(timeBeforeAction);
        Application.Quit();
    }

    void ShowMenu()
    {
        //menuContent.DOFade(1f, tweenDuration).SetEase(tweenEase);
        menuTransform.DOKill();
        menuTransform.DOAnchorPos(menuTargetPosition, tweenDuration).SetEase(tweenEase);
        menuContent.interactable = true;

        settingsTransform.DOKill();
        settingsTransform.DOAnchorPos(settingsTargetPosition, tweenDuration).SetEase(tweenEase);
        settingsContent.interactable = true;

        isShown = true;


        audioSource.PlayOneShot(openMenu);
    }

    void HideMenu()
    {
        //menuContent.DOFade(0f, tweenDuration).SetEase(tweenEase);
        menuTransform.DOKill();
        menuTransform.DOAnchorPos(menuDefaultPosition, tweenDuration).SetEase(tweenEase);
        menuContent.interactable = false;

        settingsTransform.DOKill();
        settingsTransform.DOAnchorPos(settingsDefaultPosition, tweenDuration).SetEase(tweenEase);
        settingsContent.interactable = false;

        isShown = false;

        audioSource.PlayOneShot(closeMenu);
    }

    void HideDirectMenu()
    {
        //menuContent.DOFade(0f, 0f);
        menuTransform.DOKill();
        menuTransform.DOAnchorPos(menuDefaultPosition, 0f).SetEase(tweenEase);
        menuContent.interactable = false;

        settingsTransform.DOKill();
        settingsTransform.DOAnchorPos(settingsDefaultPosition, 0f).SetEase(tweenEase);
        settingsContent.interactable = false;

        isShown = false;
    }
}
