public enum ScreenPosition
{
    TOP, BOTTOM, LEFT, RIGHT, MIDDLE
}

public static class ScreenPositionUtils
{
    public static ScreenPosition GetOppositePosition(ScreenPosition screenPosition)
    {
        ScreenPosition pos = ScreenPosition.MIDDLE;

        if (screenPosition == ScreenPosition.BOTTOM)
            pos = ScreenPosition.TOP;
        else if (screenPosition == ScreenPosition.TOP)
            pos = ScreenPosition.BOTTOM;
        else if (screenPosition == ScreenPosition.LEFT)
            pos = ScreenPosition.RIGHT;
        else if (screenPosition == ScreenPosition.RIGHT)
            pos = ScreenPosition.LEFT;

        return pos;
    }

    public static ScreenPosition GetOppositePosition(ScreenPosition screenPosition, bool horizontal = false, bool vertical = false)
    {
        if(horizontal)
        {
            if (screenPosition == ScreenPosition.LEFT || screenPosition == ScreenPosition.RIGHT)
                return GetOppositePosition(screenPosition);

        } else if (vertical)
        {
            if (screenPosition == ScreenPosition.BOTTOM || screenPosition == ScreenPosition.TOP)
                return GetOppositePosition(screenPosition);
        }

        return ScreenPosition.MIDDLE;
    }
}