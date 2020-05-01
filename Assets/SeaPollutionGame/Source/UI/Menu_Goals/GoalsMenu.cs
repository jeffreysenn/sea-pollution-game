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

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip slideInClip = null;
    [SerializeField]
    private AudioClip slideOutClip = null;
    [SerializeField]
    private List<AudioClip> completionClips = null;
    private int currentClipIndex = 0;

    private Vector2 defaultDetailsPosition = Vector2.zero;

    public event Action<GoalsMenu> OnClick;

    private bool _isShown = false;
    public bool isShown { get { return _isShown; } }

    private List<GoalItem> goalItems = new List<GoalItem>();

    WorldStateManager worldStateManager = null;

    Player player1 = null; PlayerState player1State = null;
    Player player2 = null; PlayerState player2State = null;

    int player1GoalsCount = 0;
    int player2GoalsCount = 0;

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

            goalItem.completionClip = completionClips[currentClipIndex];
            currentClipIndex = ((currentClipIndex + 1) % completionClips.Count);


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

        for(int i = 0; i < goalItems.Count; i++)
        {
            GoalItem gi = goalItems[i];
            Goal g = gi.GetGoal();

            a = worldStateManager.GetPlayerState(player1.id).HasMetGoal(g);
            b = worldStateManager.GetPlayerState(player2.id).HasMetGoal(g);

            if (a && !gi.IsLeftCompleted())
            {
                audioSource.Stop();
                if (player1GoalsCount < completionClips.Count)
                {
                    audioSource.clip = completionClips[player1GoalsCount];
                    player1GoalsCount++;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning("GoalsMenu: doesn't have sound for completion: " + player1GoalsCount);
                }
            }

            if(b && !gi.IsRightCompleted())
            {
                audioSource.Stop();
                if(player2GoalsCount < completionClips.Count)
                {
                    audioSource.clip = completionClips[player2GoalsCount];
                    player2GoalsCount++;
                    audioSource.Play();
                } else
                {
                    Debug.LogWarning("GoalsMenu: doesn't have sound for completion: " + player2GoalsCount);
                }
            }

            gi.SetValues(worldStateManager.GetPlayerState(player1.id).GetProgress(g), 
                         worldStateManager.GetPlayerState(player2.id).GetProgress(g));

            gi.Show(a, b);
        }
    }

    public void Show()
    {
        if (_isShown) return;

        _isShown = true;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(detailsTargetPosition, tweenDuration).SetEase(tweenEase);


        audioSource.PlayOneShot(slideInClip);
    }

    public void Hide()
    {
        if (!_isShown) return;

        _isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, tweenDuration).SetEase(tweenEase);


        audioSource.PlayOneShot(slideOutClip);
    }

    public void HideDirect()
    {
        _isShown = false;

        detailsTransform.DOKill();
        detailsTransform.DOAnchorPos(defaultDetailsPosition, 0f).SetEase(tweenEase);
    }
}
