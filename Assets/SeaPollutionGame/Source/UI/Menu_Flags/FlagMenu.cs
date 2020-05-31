using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class FlagMenu : MonoBehaviour
{
    [System.Serializable]
    class DifficultySetting
    {
        public string title = "";
        [Range(0f, 1f)]
        public float frequency = 0.1f;
        public bool defaultSetting = false;
    }

    [Header("Flags")]
    [SerializeField]
    private List<FlagIcon> flagsPrefab = null;
    [SerializeField]
    private RectTransform targetTransform = null;
    [SerializeField]
    private CanvasGroup menuCanvas = null;
    [SerializeField]
    private Button btnStart = null;

    [SerializeField]
    private GameObject countrySelected = null;

    [Header("Tween")]
    [SerializeField]
    private float tweenDuration = 0.5f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    [Header("Difficulty")]
    [SerializeField]
    private Slider difficultySlider = null;
    [SerializeField]
    private TextMeshProUGUI difficultyText = null;
    [SerializeField]
    private List<DifficultySetting> difficultySettings = new List<DifficultySetting>();

    private List<FlagIcon> flags = null;
    private FlagIcon flagSelected = null;
    private DisasterManager disasterManager = null;

    public event Action<CountryType> OnStart;

    private void Start()
    {
        countrySelected.SetActive(false);
        disasterManager = UIManager.Instance.disasterManager;

        GenerateFlags();

        btnStart.onClick.AddListener(BtnStart_OnClick);
        btnStart.interactable = false;

        difficultySlider.maxValue = difficultySettings.Count - 1;
        for (int i = 0; i < difficultySettings.Count; i++)
        {
            if (difficultySettings[i].defaultSetting)
            {
                difficultySlider.value = i;
                break;
            }
        }
        difficultySlider.onValueChanged.AddListener(DifficultySlider_OnValueChanged);
    }

    private void OnDestroy()
    {
        btnStart.onClick.RemoveListener(BtnStart_OnClick);
        difficultySlider.onValueChanged.RemoveListener(DifficultySlider_OnValueChanged);

        foreach(FlagIcon icon in flags)
        {
            icon.OnClick -= FlagIcon_OnClick;
        }
    }

    private void GenerateFlags()
    {
        List<FlagIcon> tempFlags = new List<FlagIcon>(flagsPrefab);

        Util.Shuffle(tempFlags);

        flags = new List<FlagIcon>();
        foreach(FlagIcon icon in tempFlags)
        {
            FlagIcon temp = Instantiate(icon, targetTransform);
            temp.OnClick += FlagIcon_OnClick;
            temp.ShowText();
            flags.Add(temp);
        }

    }

    private void FlagIcon_OnClick(FlagIcon obj)
    {
        foreach(FlagIcon icon in flags)
        {
            if(icon == obj)
            {
                flagSelected = icon;
                icon.Select();
            } else
            {
                icon.Deselect();
            }
        }
        btnStart.interactable = true;
    }

    private void BtnStart_OnClick()
    {
        if(flagSelected != null)
        {
            UIManager.Instance.baseEmissionManager.SetCountry(GetSelectedFlag().ToString().ToUpper());

            FlagIcon ingameFlag = (FlagIcon) Instantiate(flagSelected, countrySelected.transform);
            ingameFlag.HideDirectSelected();
            ingameFlag.isClickable = false;
            ingameFlag.HideText();

            RectTransform rectTransformFlag = ingameFlag.GetComponent<RectTransform>();
            rectTransformFlag.anchorMin = new Vector2(0, 0);
            rectTransformFlag.anchorMax = new Vector2(1, 1);

            rectTransformFlag.offsetMin = Vector2.zero;
            rectTransformFlag.offsetMax = Vector2.zero;
            
            countrySelected.SetActive(true);

            Hide();

            OnStart?.Invoke(GetSelectedFlag());
        }

    }

    private void DifficultySlider_OnValueChanged(float value)
    {
        int index = Mathf.RoundToInt(value);

        DifficultySetting difficultySetting = difficultySettings[index];

        foreach(Disaster d in disasterManager.GetDisasters())
        {
            d.chancePerTurn = difficultySetting.frequency;
        }

        difficultyText.text = difficultySetting.title;
    }

    public CountryType GetSelectedFlag() {

        if(flagSelected == null) { Debug.LogError("[FlagMenu] GetSelectedFlag: no flag selected");}

        return flagSelected.GetFlagType();
    }

    public void Show()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
        menuCanvas.blocksRaycasts = true;
        menuCanvas.interactable = true;
    }

    public void Hide()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

        menuCanvas.blocksRaycasts = false;
        menuCanvas.interactable = false;
    }

    public void HideDirect()
    {
        menuCanvas.DOKill();
        menuCanvas.DOFade(0f, 0f).SetEase(tweenEase);

        menuCanvas.blocksRaycasts = false;
        menuCanvas.interactable = false;
    }
}
