using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDescription : MonoBehaviour
{
    [SerializeField] public Text descriptionText = null;

    protected string description = "";

    PollutionMap pollutionMap = null;
    PieController pieController = null;

    public void SetDescription(string des) { description = des; }

    public void SetPollutionMap(PollutionMap map) { pollutionMap = map; }


    void Start()
    {
        //if (!descriptionText) { descriptionText = GameObject.Find("ItemDescription").GetComponent<Text>(); }
        //pieController = GameObject.Find("ItemDescriptionPie").GetComponent<PieController>();
    }

    protected void OnMouseEnter()
    {
        if (descriptionText)
        {
            descriptionText.text = description;
        }

        if (pieController)
        {
            pieController.SetPollutionMap(pollutionMap);
            pieController.Draw();
        }

    }

    protected void OnMouseExit()
    {
        if (descriptionText)
        {
            descriptionText.text = "";
        }

        if (pieController)
        {
            pieController.Clear();
        }
    }
}
