using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialContent : PopUpContent
{
    [SerializeField]
    private TextMeshProUGUI textTitle = null;
    [SerializeField]
    private TextMeshProUGUI textDescription = null;
    [SerializeField]
    private LayoutGroup layoutGroup = null;
    //video
    
    public bool CheckGraphicTutorial(TutorialArea tutorialArea)
    {
        bool hasFoundData = false;

        if (tutorialArea != null)
        {
            hasFoundData = true;

            textTitle.text = tutorialArea.title;
            textDescription.text = tutorialArea.description;
        }

        imageToShow = false;

        return hasFoundData;
    }
}
