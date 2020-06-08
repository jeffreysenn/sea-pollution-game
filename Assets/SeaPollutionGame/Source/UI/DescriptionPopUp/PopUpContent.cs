using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class PopUpContent : MonoBehaviour
{
    public ScreenPosition defaultAnchor = ScreenPosition.LEFT;
    public ScreenPosition currentAnchor = ScreenPosition.MIDDLE;
    public CanvasGroup canvas = null;
    public bool isShown { get; set; }
    public bool imageToShow { get; set; }
    public bool imageIsDisaster { get; set; }

    [Header("Tween")]
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;
    
    public DescriptionPopUp parentPopUp { protected get; set; }

    public virtual void ShowPopup()
    {
        if (!isShown)
        {
            //content.canvas.DOKill();
            canvas.DOFade(1f, tweenDuration).SetEase(tweenEase);

            isShown = true;
        }
    }

    public virtual void HidePopup(bool instant = false)
    {
        if(instant)
        {
            canvas.DOFade(0f, 0f);
            isShown = false;
            return;
        }

        if (isShown)
        {
            //content.canvas.DOKill();
            canvas.DOFade(0f, tweenDuration).SetEase(tweenEase);

            isShown = false;
        }
    }

    public virtual void HideDirectPopup()
    {
        canvas.DOKill();
        HidePopup(true);
        isShown = false;
    }
}
