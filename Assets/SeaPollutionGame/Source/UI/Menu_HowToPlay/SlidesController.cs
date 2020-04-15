using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlidesController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> slides = new List<GameObject>();

    [SerializeField]
    private Button btnPrevious = null;
    [SerializeField]
    private CanvasGroup canvasPrevious = null;

    [SerializeField]
    private Button btnNext = null;
    [SerializeField]
    private CanvasGroup canvasNext = null;

    [Header("Transition")]
    [SerializeField]
    private float tweenDuration = 0.25f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private GameObject currentSlide = null;
    private int currentIndex = 0;

    private void Start()
    {
        btnPrevious.onClick.AddListener(PreviousSlide);
        btnNext.onClick.AddListener(NextSlide);

        foreach(GameObject g in slides)
        {
            Image i = g.GetComponentInChildren<Image>();

            if(i!=null)
            {
                i.DOFade(0f, 0f);
            }
        }

        currentSlide = slides[currentIndex];

        ShowSlide(slides[currentIndex]);
    }

    void NextSlide()
    {
        currentIndex = (currentIndex + 1) % slides.Count;
        ShowSlide(slides[currentIndex]);
    }

    void PreviousSlide()
    {
        currentIndex = (currentIndex - 1) % slides.Count;
        ShowSlide(slides[currentIndex]);
    }

    void ShowSlide(GameObject slide)
    {
        Image currentImage = currentSlide.GetComponentInChildren<Image>();
        if(currentImage != null)
        {
            currentImage.DOFade(0f, tweenDuration).SetEase(tweenEase);
        }
        //currentSlide.SetActive(false);

        currentSlide = slide;
        currentImage = currentSlide.GetComponentInChildren<Image>();
        if (currentImage != null)
        {
            currentImage.DOFade(1f, tweenDuration).SetEase(tweenEase);
        }

        //slide.SetActive(true);

        int index = slides.IndexOf(slide);

        canvasPrevious.alpha = (index == 0) ? 0 : 1;
        canvasPrevious.interactable = (canvasPrevious.alpha != 0);

        canvasNext.alpha = (index == slides.Count - 1) ? 0 : 1;
        canvasNext.interactable = (canvasNext.alpha != 0);
    }
}
