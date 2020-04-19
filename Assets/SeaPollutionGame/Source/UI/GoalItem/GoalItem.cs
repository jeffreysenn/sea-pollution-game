using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;

public class GoalItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject leftBorder = null;
    [SerializeField]
    private GameObject rightBorder = null;
    [SerializeField]
    private GameObject glow = null;
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private bool hoverable = false;

    [SerializeField]
    private TextMeshProUGUI title = null;

    private Goal goal;
    public void SetGoal(Goal g) { goal = g; }
    public Goal GetGoal() { return goal; }

    [SerializeField]
    private bool isLeftShown = false;
    [SerializeField]
    private bool isRightShown = false;

    public AudioClip completionClip = null;

    public event Action<GoalItem> OnClick;

    public bool IsLeftCompleted() { return isLeftShown; }
    public bool IsRightCompleted() { return isRightShown; }
    public bool IsCompleted() { return isLeftShown && isRightShown; }

    private float _valueLeft = 0f;
    public float valueLeft { get { return _valueLeft; } }
    private float _valueRight = 0f;
    public float valueRight { get { return _valueRight; } }

    private void Start()
    {
        Show(isLeftShown, isRightShown);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }

    public void SetValues(float left, float right)
    {
        _valueLeft = left;
        _valueRight = right;
    }

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetImage(Sprite s)
    {
        image.overrideSprite = s;
    }

    public void Show(bool left = false, bool right = false)
    {
        leftBorder.SetActive(left);
        rightBorder.SetActive(right);

        isLeftShown = left;
        isRightShown = right;

    }

    public void Hide(bool left = false, bool right = false)
    {
        leftBorder.SetActive(!left);
        rightBorder.SetActive(!right);

        isLeftShown = !left;
        isRightShown = !right;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(hoverable)
            glow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(hoverable)
            glow.SetActive(false);
    }
}
