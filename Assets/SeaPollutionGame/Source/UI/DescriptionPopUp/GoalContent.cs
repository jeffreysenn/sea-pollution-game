using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalContent : PopUpContent
{
    [SerializeField]
    private TextMeshProUGUI textTitle = null;
    [SerializeField]
    private TextMeshProUGUI textResourceName = null;
    [SerializeField]
    private TextMeshProUGUI textDescription = null;
    [SerializeField]
    private TextMeshProUGUI textReward = null;
    [SerializeField]
    private CustomBarChart barPlayerAProgress = null;
    [SerializeField]
    private CustomBarChart barPlayerBProgress = null;
    
    public bool CheckGraphicGoal(GoalItem goalItem)
    {
        bool hasFoundData = false;

        Goal g = goalItem.GetGoal();
        if (g != null)
        {
            hasFoundData = true;

            textTitle.text = g.title;
            textDescription.text = g.description;

            textResourceName.text = g.resourceName;
            textReward.text = g.reward.ToString();

            int thresholdA = barPlayerAProgress.GetThreshold();
            bool withThresholdA = true;

            if(goalItem.valueLeft > (thresholdA / 100))
            {
                withThresholdA = false;
            }

            int thresholdB = barPlayerBProgress.GetThreshold();
            bool withThresholdB = true;

            if (goalItem.valueLeft > (thresholdB / 100))
            {
                withThresholdB = false;
            }

            barPlayerAProgress.SetValues(goalItem.valueLeft, 1 - goalItem.valueLeft, true, withThresholdA);
            barPlayerBProgress.SetValues(goalItem.valueRight, 1 - goalItem.valueRight, true, withThresholdB);
        }

        imageToShow = false;

        return hasFoundData;
    }
}
