using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;
using TMPro;
using DG.Tweening;

public class GraphController : MonoBehaviour
{
    [System.Serializable]
    class StringBlackboard
    {
        public string name;
        public PlayerStateBlackboard.Key key;
        public string description;
        [Header("only if key is Pollution")]
        public PollutionMapType pollutionType;
        [Header("only if key is Resource")]
        public string resourceName;
    }

    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private GraphChart graph = null;

    [Header("Disaster")]
    [SerializeField]
    private RectTransform disasterTransform = null;
    [SerializeField]
    private GameObject disasterLinePrefab = null;
    private List<GameObject> disasterLines = new List<GameObject>();

    [Header("Dropdown")]
    [SerializeField]
    private List<StringBlackboard> dropDownValues = new List<StringBlackboard>();
    [SerializeField]
    private TMP_Dropdown dropDown = null;
    [SerializeField]
    private Sprite dropDownImageOpened = null;
    private Sprite dropDownImageDefault = null;
    [SerializeField]
    private TextMeshProUGUI dropDownDescription = null;

    [Header("Tween")]
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    [Header("Hover Item")]
    [SerializeField]
    private RectTransform hoverItem = null;
    [SerializeField]
    private CanvasGroup canvasHoverItem = null;
    [SerializeField]
    private TextMeshProUGUI txtHoverItem = null;
    [SerializeField]
    private Vector2 hoverOffset = Vector2.zero;
    
    private WorldStateManager worldStateManager = null;
    private DisasterManager disasterManager = null;

    private Dictionary<int, StateHistory> histories = new Dictionary<int, StateHistory> { };
    private Dictionary<int, Disaster> disastersHistory = new Dictionary<int, Disaster>();
    private int totalTurnCount = 0;

    private void Start()
    {
        HideDirect();
        HideDirectItem();

        worldStateManager = UIManager.Instance.worldStateManager;
        disasterManager = UIManager.Instance.disasterManager;
        totalTurnCount = worldStateManager.GetRemainingTurnCount();

        dropDownImageDefault = dropDown.image.sprite;

        List<string> s = new List<string>();
        foreach (StringBlackboard sb in dropDownValues)
            s.Add(sb.name);
        dropDown.AddOptions(s);

        worldStateManager.GetEndTurnEvent().AddListener(OnEndTurn);
        dropDown.onValueChanged.AddListener(OnDropdownChanged);

        disasterManager.AddDisasterEventListener(OnDisaster);
        disasterManager.AddNoDisasterEventListener(OnNoDisaster);
    }

    private void Update()
    {
        if(dropDown.IsExpanded)
        {
            dropDown.image.overrideSprite = dropDownImageOpened;
        } else
        {
            dropDown.image.overrideSprite = dropDownImageDefault;
        }

        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            graph.gameObject.SetActive(!graph.gameObject.activeInHierarchy);
            background.gameObject.SetActive(!background.gameObject.activeInHierarchy);
        }

        if (graph.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Plot(PlayerStateBlackboard.Key.MONEY);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                Plot(PlayerStateBlackboard.Key.ASSET_VALUE);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                Plot(PlayerStateBlackboard.Key.POLLUTION, PollutionMapType.NET, "Emission");
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                Plot(PlayerStateBlackboard.Key.RESOURCE, "SPG 13");

        }
        */
    }

    private void OnEndTurn()
    {
        OnDropdownChanged(dropDown.value);
    }

    private void OnDropdownChanged(int v)
    {
        PlayerStateBlackboard.Key key = dropDownValues[v].key;

        dropDownDescription.text = dropDownValues[v].description;

        if(key == PlayerStateBlackboard.Key.POLLUTION)
        {
            Plot(key, dropDownValues[v].pollutionType, "Emission");
        } else if (key == PlayerStateBlackboard.Key.RESOURCE)
        {
            Plot(key, dropDownValues[v].resourceName);
        } else
        {
            Plot(key);
        }
    }

    private void OnDisaster(Disaster d)
    {
        disastersHistory.Add(totalTurnCount - worldStateManager.GetRemainingTurnCount(), d);
    }

    private void OnNoDisaster()
    {
        disastersHistory.Add(totalTurnCount - worldStateManager.GetRemainingTurnCount(), null);
    }

    public void SetStateHistory(int id, StateHistory history) { histories[id] = history; }

    public void Plot(PlayerStateBlackboard.Key key)
    {
        Plot(blackboard => blackboard.GetValue(key));
    }

    public void Plot(PlayerStateBlackboard.Key key, PollutionMapType pollutionMapType, string pollutionName)
    {
        Plot(blackboard =>
        {
            var val = blackboard.GetValue(key, pollutionMapType, pollutionName);
            val = val < 0 ? -val : val;
            return val;
        });
    }

    public void Plot(PlayerStateBlackboard.Key key, string resourceName)
    {
        Plot(blackboard => blackboard.GetValue(key, resourceName));
    }

    private void Plot(Func<PlayerStateBlackboard, float> getValueFuc)
    {
        graph.DataSource.StartBatch();
        var pids = worldStateManager.GetPlayerIDs();
        foreach (var playerID in pids)
        {
            var category = "Player " + playerID.ToString();

            graph.DataSource.ClearCategory(category);

            var blackboard = worldStateManager.GetStateHistory(playerID);

            for (int i = 0; i != blackboard.Count; ++i)
            {
                float val = getValueFuc.Invoke(blackboard[i]);

                graph.DataSource.AddPointToCategory(category, i + 1, val);
            }
        }
        graph.DataSource.EndBatch();

        //clear already existing lines
        foreach(Transform child in disasterTransform)
        {
            Destroy(child.gameObject);
        }

        disasterLines = new List<GameObject>();
        List<int> disasterInstantied = new List<int>();

        //add lines
        foreach (var playerID in pids)
        {
            var category = "Player " + playerID.ToString();
            var blackboard = worldStateManager.GetStateHistory(playerID);

            for (int i = 0; i != blackboard.Count; ++i)
            {
                if (disastersHistory.ContainsKey(i) && disastersHistory[i] != null && !disasterInstantied.Contains(i))
                {
                    GameObject newLine = Instantiate(disasterLinePrefab, disasterTransform);

                    RectTransform lineTransform = newLine.GetComponent<RectTransform>();
                    TextMeshProUGUI lineText = lineTransform.GetComponentInChildren<TextMeshProUGUI>();
                    lineText.text = disastersHistory[i].title;

                    ChartAndGraph.DoubleVector3 point = graph.DataSource.GetPoint(category, i);
                    Vector3 pos = Vector3.zero;
                    graph.PointToWorldSpace(out pos, point.x, point.y);

                    lineTransform.position = new Vector3(pos.x, lineTransform.position.y, lineTransform.position.z);

                    disasterLines.Add(newLine);
                    disasterInstantied.Add(i);
                }
            }
        }
    }

    public void Show()
    {
        OnDropdownChanged(dropDown.value);
        canvasGroup.DOFade(1f, tweenDuration).SetEase(tweenEase);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.DOFade(0f, tweenDuration).SetEase(tweenEase);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void HideDirect()
    {
        canvasGroup.DOFade(0f, 0f).SetEase(tweenEase);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowItem(GraphChart.GraphEventArgs args)
    {
        txtHoverItem.text = String.Format("{0:0}", args.Value.y);
        LayoutRebuilder.ForceRebuildLayoutImmediate(hoverItem);

        Vector3 pos = Vector3.zero;
        graph.PointToWorldSpace(out pos, args.Value.x, args.Value.y);
        hoverItem.position = new Vector2(pos.x + hoverOffset.x, pos.y + hoverOffset.y);

        canvasHoverItem.DOFade(1f, tweenDuration).SetEase(tweenEase);
    }

    public void HideItem()
    {
        canvasHoverItem.DOFade(0f, tweenDuration).SetEase(tweenEase);
    }

    public void HideDirectItem()
    {
        canvasHoverItem.DOFade(0f, 0f);
    }
}
