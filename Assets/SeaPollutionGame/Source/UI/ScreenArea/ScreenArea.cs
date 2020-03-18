using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenArea : MonoBehaviour
{
    [SerializeField]
    private RectTransform top = null;
    [SerializeField]
    private RectTransform bot = null;
    [SerializeField]
    private RectTransform left = null;
    [SerializeField]
    private RectTransform right = null;

    public ScreenPosition GetPosition(Vector2 point)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(top, point))
        {
            return ScreenPosition.TOP;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(bot, point))
        {
            return ScreenPosition.BOTTOM;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(left, point))
        {
            return ScreenPosition.LEFT;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(right, point))
        {
            return ScreenPosition.RIGHT;
        }

        return ScreenPosition.MIDDLE;
    }
}
