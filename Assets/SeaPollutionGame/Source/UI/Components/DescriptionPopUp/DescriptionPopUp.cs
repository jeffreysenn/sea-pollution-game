using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DescriptionPopUp : MonoBehaviour
{
    /*
        * DescriptionPopUp:
        *  Appears when hovering over Polluter, Flow, Node, Sea with corresponding Content
        *  
        *  
        * TODO: Raycast on specific tag colliders, not on DrawDescription
    */

    class PopUpContent
    {
        public CanvasGroup canvas = null;
        public bool isShown { get; set; }
    }

    [System.Serializable]
    class PolluterContent : PopUpContent
    {
        public TextMeshProUGUI textTitle = null;
        public PieChartController pieChart = null;
        public TextMeshProUGUI textDetails = null;
        public TextMeshProUGUI textVulnerabilities = null;
    }

    [System.Serializable]
    class NodeContent : PopUpContent
    {
        public PieChartController pieChart = null;
    }

    [System.Serializable]
    class BalticContent : PopUpContent
    {
        public PieChartController pieChart = null;
    }

    [SerializeField]
    private GameObject popUp = null;
    
    [SerializeField]
    private PolluterContent polluterContent = null;

    [SerializeField]
    private NodeContent nodeContent = null;

    [SerializeField]
    private BalticContent balticContent = null;
    [SerializeField]
    private string balticTag = "Sea";
    
    [SerializeField]
    private float tweenDuration = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    private List<PopUpContent> allPopupContents = new List<PopUpContent>();
    private GameObject currentGameObject = null;
    private PopUpContent currentShownContent = null;

    private void Start()
    {
        allPopupContents.Add(polluterContent);
        allPopupContents.Add(nodeContent);
        allPopupContents.Add(balticContent);
        
        HideDirectPopup(polluterContent);
        HideDirectPopup(nodeContent);
        HideDirectPopup(balticContent);
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
                if(hit.transform.gameObject != currentGameObject)
                {
                    //ClearFields();
                    
                    currentGameObject = hit.transform.gameObject;
                    
                    if (CheckPolluter(currentGameObject))
                    {
                        HidePopupOtherThan(polluterContent);
                        ShowPopup(polluterContent);
                    }
                    else if (CheckBalticSea(currentGameObject))
                    {
                        HidePopupOtherThan(balticContent);
                        ShowPopup(balticContent);
                    }
                    else if (CheckNode(currentGameObject))
                    {
                        HidePopupOtherThan(nodeContent);
                        ShowPopup(nodeContent);
                    }
                    else if (CheckFlow(currentGameObject))
                    {
                        HidePopupOtherThan(nodeContent);
                        ShowPopup(nodeContent);
                    }

                }
            }
        } else
        {
            currentGameObject = null;
            if(currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }
        }
    }

    private bool CheckPolluter(GameObject targetGameObject)
    {
        bool hasFoundData = false;

        Polluter polluter = targetGameObject.GetComponentInChildren<Polluter>();
        if (polluter != null)
        {
            hasFoundData = true;
            PolluterAttrib attrib = polluter.GetAttrib();

            polluterContent.textTitle.text = attrib.title;

            polluterContent.textDetails.text = "Price: " + attrib.economicAttrib.price + "\nCost: " + attrib.economicAttrib.profitPerTurn + "\nRemove: " + attrib.economicAttrib.removalCost;

            polluterContent.textVulnerabilities.text = attrib.vulnerabilityAttrib.GetDescription();

            PollutionMap map = new PollutionMap(attrib.pollutionAttrib.emissions);

            if(polluter is Filter)
            {
                map = Util.MultiplyMap(map, (-1));
            }

            SetPieChart(polluterContent.pieChart, map);
        }

        return hasFoundData;
    }

    private bool CheckFlow(GameObject targetGameObject)
    {
        bool hasFoundData = false;

        Flow flow = targetGameObject.GetComponentInChildren<Flow>();
        if (flow != null)
        {
            PollutionMap map = flow.GetPollutionMap();

            float total = map.GetTotalPollution();

            if (total < 0)
            {
                hasFoundData = true;

                SetPieChart(nodeContent.pieChart, Util.MultiplyMap(map, -1));
            } else if (total > 0)
            {
                hasFoundData = true;

                SetPieChart(nodeContent.pieChart, map);
            }
        }

        return hasFoundData;
    }

    private bool CheckNode(GameObject targetGameObject)
    {
        bool hasFoundData = false;

        Node node = targetGameObject.GetComponentInChildren<Node>();
        if (node != null)
        {
            PollutionMap map = node.GetPollutionMap();

            float total = map.GetTotalPollution();

            if (total < 0)
            {
                hasFoundData = true;

                SetPieChart(nodeContent.pieChart, Util.MultiplyMap(map, -1));
            }
            else if (total > 0)
            {
                hasFoundData = true;

                SetPieChart(nodeContent.pieChart, map);
            }
        }

        return hasFoundData;
    }

    private bool CheckBalticSea(GameObject targetGameObject)
    {
        bool hasFoundData = false;

        Node node = targetGameObject.GetComponentInChildren<Node>();
        if (node != null)
        {
            if(node.CompareTag(balticTag))
            {
                hasFoundData = true;

                PollutionMap map = node.GetPollutionMap();
                
                SetPieChart(balticContent.pieChart, map);
            }
        }

        return hasFoundData;
    }

    private void SetPieChart(PieChartController pieChart, PollutionMap map)
    {
        pieChart.Clear();
        pieChart.SetPollutionMap(map);
        pieChart.Draw();
    }

    private void ShowPopup(PopUpContent content)
    {
        currentShownContent = content;

        if (!content.isShown)
        {
            content.canvas.DOKill();
            content.canvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
            content.isShown = true;
        }
    }

    private void HidePopup(PopUpContent content)
    {
        currentShownContent = null;

        if (content.isShown)
        {
            content.canvas.DOKill();
            content.canvas.DOFade(0f, tweenDuration).SetEase(tweenEase);
            content.isShown = false;
        }
    }

    private void HideDirectPopup(PopUpContent content)
    {
        currentShownContent = null;

        content.canvas.DOKill();
        content.canvas.DOFade(0f, 0f).SetEase(tweenEase);
        content.isShown = false;
    }

    private void HidePopupOtherThan(PopUpContent content)
    {
        foreach(PopUpContent puc in allPopupContents)
        {
            if(puc != content)
            {
                HideDirectPopup(puc);
            }
        }
    }
}
