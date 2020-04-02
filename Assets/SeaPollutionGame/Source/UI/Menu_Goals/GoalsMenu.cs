using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Linq;

public class GoalsMenu : MonoBehaviour
{
    [System.Serializable]
    class GoalLine
    {
        public Transform lineTransform = null;
        public int numberPerLine = 3;
        private int _currentNumber = 0;
        public int currentNumber
        {
            get { return _currentNumber; }
            set { _currentNumber = value; }
        }
    }

    [SerializeField]
    private GoalItem mainGoalItem = null;

    [SerializeField]
    private GoalItem goalItemPrefab = null;

    [Header("Lines")]
    [SerializeField]
    private List<GoalLine> goalLines = null;

    [Header("Images")]
    [SerializeField]
    private string folderPath = "Images/Goals/";

    [Header("Tween")]
    [SerializeField]
    private RectTransform detailsTransform = null;
    [SerializeField]
    private Vector2 detailsTargetPosition = Vector2.zero;
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private Vector2 defaultDetailsPosition = Vector2.zero;

    public event Action<GoalsMenu> OnClick;

    private bool _isShown = false;
    public bool isShown { get { return _isShown; } }

    private List<GoalItem> goalItems = new List<GoalItem>();

    WorldStateManager worldStateManager = null;

    Player player1 = null; PlayerState player1State = null;
    Player player2 = null; PlayerState player2State = null;

    private void Start()
    {
        worldStateManager = UIManager.Instance.worldStateManager;

        player1 = UIManager.Instance.player1;
        player2 = UIManager.Instance.player2;

        player1State = worldStateManager.GetPlayerState(player1.id);
        player2State = worldStateManager.GetPlayerState(player2.id);

        //load images from resources
        List<Sprite> sprites = Resources.LoadAll(folderPath, typeof(Sprite)).Cast<Sprite>().ToList();

        AttribData attribData = UIManager.Instance.attribLoader.LoadLazy();

        foreach(Goal goal in attribData.goalList)
        {
            Transform targetTransform = null;
            for(int i = 0; i < goalLines.Count; i++)
            {
                if(goalLines[i].currentNumber < goalLines[i].numberPerLine)
                {
                    targetTransform = goalLines[i].lineTransform;
                    goalLines[i].currentNumber += 1;
                    break;
                }
            }

            GoalItem goalItem = Instantiate(goalItemPrefab, targetTransform);
            goalItem.SetGoal(goal);
            goalItem.SetValues(0, 0);
            
            Sprite s = sprites.Find(x => x.name.ToLower().Equals(goal.iconName.ToLower()));
            if(s!= null)
            {
                goalItem.SetImage(s);
            }


            goalItems.Add(goalItem);

            goalItem.SetTitle(goalItems.Count.ToString());
        }
        
        defaultDetailsPosition = detailsTransform.anchoredPosition;

        mainGoalItem.OnClick += MainGoalItem_OnClick;
        mainGoalItem.SetGoal(null);

        player1State.GetResourceChangeEvent().AddListener(PlayerStateResourceChanged);
        player2State.GetResourceChangeEvent().AddListener(PlayerStateResourceChanged);
    }

    private void MainGoalItem_OnClick(GoalItem obj)
    {
        OnClick?.Invoke(this);
    }

    private void PlayerStateResourceChanged()
    {
        bool a = false;
        bool b = false;

        foreach(GoalItem gi in goalItems)
        {
            Goal g = gi.GetGoal();

            a = worldStateManager.HasPlayerMetGoal(g, player1.id);
            b = worldStateManager.HasPlayerMetGoal(g, player2.id);

            gi.SetValues(worldStateManager.GetPlayerProgress(g, player1.id), worldStateManager.GetPlayerProgress(g, player2.id));

            gi.Show(a, b);
        }
    }

    public void Show()
    {
        if (_isShown) return;

        _isShown = true;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(detailsTargetPosition, tweenDuration).SetEase(tweenEase);
    }

    public void Hide()
    {
        if (!_isShown) return;

        _isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, tweenDuration).SetEase(tweenEase);
    }

    public void HideDirect()
    {
        _isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, 0f).SetEase(tweenEase);
    }
}
