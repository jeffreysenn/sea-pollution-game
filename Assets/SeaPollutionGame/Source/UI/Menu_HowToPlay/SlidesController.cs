using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject currentSlide = null;
    private int currentIndex = 0;

    private void Start()
    {
        btnPrevious.onClick.AddListener(PreviousSlide);
        btnNext.onClick.AddListener(NextSlide);

        foreach(GameObject g in slides)
        {
            g.SetActive(false);
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
        currentSlide.SetActive(false);

        currentSlide = slide;

        slide.SetActive(true);

        int index = slides.IndexOf(slide);

        canvasPrevious.alpha = (index == 0) ? 0 : 1;
        canvasNext.alpha = (index == slides.Count - 1) ? 0 : 1;
    }
}
