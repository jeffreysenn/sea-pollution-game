using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopUpContent : MonoBehaviour
{
    public ScreenPosition defaultAnchor = ScreenPosition.LEFT;
    public CanvasGroup canvas = null;
    public bool isShown { get; set; }
    public bool imageToShow { get; set; }
    public bool imageIsDisaster { get; set; }
}
