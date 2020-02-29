using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DescriptionPopUp : MonoBehaviour
{
    /*
     * TODO: Raycast on specific components, not on DrawDescription
     *      Tween show and hide
     */

    [SerializeField]
    private GameObject popUp = null;

    [SerializeField]
    private float tweenDuration = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private CanvasGroup canvasPopup = null;
    private bool isShown = false;

    private void Start()
    {
        canvasPopup = popUp.GetComponentInChildren<CanvasGroup>();

        HidePopup();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.GetComponent<DrawDescription>() != null)
            {
                //update content

                if(!isShown)
                {
                    ShowPopup();
                }
            }
        } else
        {
            if(isShown)
            {
                HidePopup();
            }
        }
    }

    private void ShowPopup()
    {
        canvasPopup.DOFade(1f, tweenDuration).SetEase(tweenEase);

        isShown = true;
    }

    private void HidePopup()
    {
        canvasPopup.DOFade(0f, tweenDuration).SetEase(tweenEase);
        
        isShown = false;
    }

    private void HideDirectPopup()
    {
        canvasPopup.DOFade(0f, 0f).SetEase(tweenEase);

        isShown = false;
    }
}
