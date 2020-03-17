using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GoalItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject leftBorder = null;
    [SerializeField]
    private GameObject rightBorder = null;

    private Goal goal;
    public void SetGoal(Goal g) { goal = g; }
    public Goal GetGoal() { return goal; }

    [SerializeField]
    private bool isLeftShown = false;
    [SerializeField]
    private bool isRightShown = false;

    public event Action<GoalItem> OnClick;

    private void Start()
    {
        Show(isLeftShown, isRightShown);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
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
}
