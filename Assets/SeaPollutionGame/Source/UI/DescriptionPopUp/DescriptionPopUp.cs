using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [System.Serializable]
    class DisasterContent : PopUpContent
    {
        public TextMeshProUGUI textTitle = null;
    }

    [SerializeField]
    private GameObject popUp = null;

    [SerializeField]
    private WorldWindow worldWindow = null;
    
    [SerializeField]
    private PolluterContent polluterContent = null;

    [SerializeField]
    private NodeContent nodeContent = null;

    [SerializeField]
    private BalticContent balticContent = null;
    [SerializeField]
    private string balticTag = "Sea";

    [SerializeField]
    private DisasterContent disasterContent = null;
    
    [SerializeField]
    private float tweenDuration = 1f;
    [SerializeField]
    private Ease tweenEase = Ease.Linear;

    [Header("Raycast")]
    [SerializeField]
    private bool raycastPolluter = true;
    [SerializeField]
    private bool raycastBaltic = true;
    [SerializeField]
    private bool raycastNode = true;
    [SerializeField]
    private bool raycastFlow = true;
    [SerializeField]
    private bool raycastDisaster = false;
    [SerializeField]
    private string blockingRaycastTag = "BlockingUI";
    private bool isBlocked = false;

    private List<PopUpContent> allPopupContents = new List<PopUpContent>();

    private bool currentFromGame = false;
    private GameObject currentGameObject = null;
    private PopUpContent currentShownContent = null;

    private bool imageToShow = false;
    private bool imageIsDisaster = false;

    GraphicRaycaster graphicRaycaster = null;
    PointerEventData pointerEventData = null;
    EventSystem eventSystem = null;

    private PlayerController playerController = null;

    private void Start()
    {
        playerController = UIManager.Instance.playerController;

        graphicRaycaster = GetComponent<GraphicRaycaster>();
        if(graphicRaycaster == null) { Debug.LogError("[DescriptionPopUp] Start: no GraphicRaycaster found"); }
        eventSystem = EventSystem.current;
        if (eventSystem == null) { Debug.LogError("[DescriptionPopUp] Start: no EventSystem found"); }

        allPopupContents.Add(polluterContent);
        allPopupContents.Add(nodeContent);
        allPopupContents.Add(balticContent);
        allPopupContents.Add(disasterContent);

        HideDirectPopup(polluterContent);
        HideDirectPopup(nodeContent);
        HideDirectPopup(balticContent);
        HideDirectPopup(disasterContent);
    }

    private void Update()
    {
        if (playerController.GetState() == PlayerController.State.HOLDING)
        {
            if (currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }

            return;
        }

        if(Input.GetButtonDown("Fire2"))
        {
            if(currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }
        }

        /*
         * Raycast in game on click: 
         *  hide the current shown content
         *  
         *  if hit show the content at the object position
         *  if misses hide the current content
         */

        bool igRaycast = false;

        if (Input.GetButtonDown("Fire1") && !isBlocked)
        {
            if(currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }

            igRaycast = InGameRaycasting();

            if(!igRaycast)
            {
                currentGameObject = null;

                if (currentShownContent != null)
                {
                    HidePopup(currentShownContent);
                }
            } else
            {
                currentFromGame = true;
                transform.position = Camera.main.WorldToScreenPoint(currentGameObject.transform.position);
            }
        }
        
        /*
         * Raycast UI
         *  if true: hide the current shown content if any and show the new one at mouse position
         */

        bool uiRaycast = UIRaycasting();

        if(uiRaycast)
        {
            currentFromGame = false;
            transform.position = Input.mousePosition;
        }

        if(!uiRaycast && !currentFromGame)
        {
            currentGameObject = null;

            if (currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }
        }
    }

    private bool UIRaycasting()
    {
        bool hasHit = false;
        isBlocked = false;

        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> graphicResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, graphicResults);

        EventSystem.current.RaycastAll(pointerEventData, graphicResults);

        foreach (RaycastResult rr in graphicResults)
        {
            if(rr.gameObject.CompareTag(blockingRaycastTag))
            {
                isBlocked = true;
                return hasHit;
            }

            PurchasableIcon purchasableIcon = rr.gameObject.GetComponentInChildren<PurchasableIcon>();
            if (purchasableIcon != null)
            {
                hasHit = true;

                if (purchasableIcon.gameObject != currentGameObject)
                {
                    currentGameObject = purchasableIcon.gameObject;

                    if (CheckGraphicPolluter(purchasableIcon))
                    {

                        HidePopupOtherThan(polluterContent);
                        ShowPopup(polluterContent);
                    }
                }
            }

            
            if(raycastDisaster)
            {
                DisasterIcon disasterIcon = rr.gameObject.GetComponentInChildren<DisasterIcon>();
                if (disasterIcon != null)
                {
                    hasHit = true;

                    if (disasterIcon.gameObject != currentGameObject)
                    {
                        currentGameObject = disasterIcon.gameObject;

                        if (CheckGraphicDisaster(disasterIcon))
                        {
                            HidePopupOtherThan(disasterContent);
                            ShowPopup(disasterContent);
                        }
                    }
                }
            }
        }

        return hasHit;
    }

    private bool InGameRaycasting()
    {
        bool hasHit = false;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<DrawDescription>() != null)
            {
                if (hit.transform.gameObject != currentGameObject)
                {
                    currentGameObject = hit.transform.gameObject;

                    if (CheckPolluter(currentGameObject) && raycastPolluter)
                    {
                        hasHit = true;

                        HidePopupOtherThan(polluterContent);
                        ShowPopup(polluterContent);
                    }
                    else if (CheckBalticSea(currentGameObject) && raycastBaltic)
                    {
                        hasHit = true;

                        HidePopupOtherThan(balticContent);
                        ShowPopup(balticContent);
                    }
                    else if (CheckNode(currentGameObject) && raycastNode)
                    {
                        hasHit = true;

                        HidePopupOtherThan(nodeContent);
                        ShowPopup(nodeContent);
                    }
                    else if (CheckFlow(currentGameObject) && raycastFlow)
                    {
                        hasHit = true;

                        HidePopupOtherThan(nodeContent);
                        ShowPopup(nodeContent);
                    }
                }
            }
        }

        return hasHit;
    }

    private bool CheckGraphicPolluter(PurchasableIcon purchasableIcon)
    {
        bool hasFoundData = false;

        PolluterAttrib attrib = purchasableIcon.GetPolluterAttributes();
        if (attrib != null)
        {
            hasFoundData = true;

            polluterContent.textTitle.text = attrib.title;

            polluterContent.textDetails.text = "Price: " + attrib.economicAttrib.price + " Income: " + attrib.economicAttrib.profitPerTurn + "\nRemoval cost: " + attrib.economicAttrib.removalCost;

            VulnerabilityAttrib vulnerabilityAttrib = attrib.vulnerabilityAttrib;
            if(vulnerabilityAttrib.vulnerabilities != null)
            {
                string vulnerabilityString = ""; //"Vulnerable to ";
                foreach (VulnerabilityAttrib.Vulnerability v in vulnerabilityAttrib.vulnerabilities)
                {
                    vulnerabilityString += v.disasterName + ": " + v.factor + " ";
                }
                polluterContent.textVulnerabilities.text = vulnerabilityString;
            } else
            {
                polluterContent.textVulnerabilities.text = "";
            }


            PollutionMap map = new PollutionMap(attrib.pollutionAttrib.emissions);

            if (purchasableIcon.GetPolluterIcon().GetPolluter().GetEntityType() == EntityType.FILTER)
            {
                map = Util.MultiplyMap(map, (-1));
            }

            VisualAttrib visualAttrib = attrib.visualAttrib;
            if(visualAttrib.imageName != "")
            {
                worldWindow.imageLoader.LoadImage(visualAttrib.imageName);
                imageToShow = true;
            } else
            {
                imageToShow = false;
            }

            imageIsDisaster = false;

            SetPieChart(polluterContent.pieChart, map);
        }

        return hasFoundData;
    }

    private bool CheckGraphicDisaster(DisasterIcon disasterIcon)
    {
        bool hasFoundData = false;

        Disaster disaster = disasterIcon.GetDisaster();
        if(disaster != null)
        {
            hasFoundData = true;

            disasterContent.textTitle.text = disaster.title;
        }

        imageToShow = false;

        return hasFoundData;
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

            polluterContent.textDetails.text = "Price: " + attrib.economicAttrib.price + " Income: " + attrib.economicAttrib.profitPerTurn + "\nRemoval cost: " + attrib.economicAttrib.removalCost;

            VulnerabilityAttrib vulnerabilityAttrib = attrib.vulnerabilityAttrib;
            if (vulnerabilityAttrib.vulnerabilities != null)
            {
                string vulnerabilityString = "Vulnerable to ";
                foreach (VulnerabilityAttrib.Vulnerability v in vulnerabilityAttrib.vulnerabilities)
                {
                    vulnerabilityString += v.disasterName + ":" + v.factor + " ";
                }
                polluterContent.textVulnerabilities.text = vulnerabilityString;
            }
            else
            {
                polluterContent.textVulnerabilities.text = "";
            }

            PollutionMap map = new PollutionMap(attrib.pollutionAttrib.emissions);

            if(polluter is Filter)
            {
                map = Util.MultiplyMap(map, (-1));
            }
            
            VisualAttrib visualAttrib = attrib.visualAttrib;
            if (visualAttrib.imageName != "")
            {
                worldWindow.imageLoader.LoadImage(visualAttrib.imageName);
                imageToShow = true;
            } else
            {
                imageToShow = false;
            }
            
            imageIsDisaster = !polluter.IsAlive();

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

            imageToShow = false;
            imageIsDisaster = false;
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

            imageToShow = false;
            imageIsDisaster = false;
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

            imageToShow = false;
            imageIsDisaster = false;
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
            //content.canvas.DOKill();
            content.canvas.DOFade(1f, tweenDuration).SetEase(tweenEase);
            content.isShown = true;
            
        }

        if (imageToShow)
        {
            worldWindow.ShowImage(imageIsDisaster);
        }
        else
        {
            worldWindow.HideImage();
        }
    }

    private void HidePopup(PopUpContent content)
    {
        currentShownContent = null;

        if (content.isShown)
        {
            //content.canvas.DOKill();
            content.canvas.DOFade(0f, tweenDuration).SetEase(tweenEase);
            content.isShown = false;
            
            worldWindow.HideImage();
        }
    }

    private void HideDirectPopup(PopUpContent content)
    {
        currentShownContent = null;

        content.canvas.DOKill();
        content.canvas.DOFade(0f, 0f);
        content.isShown = false;

        worldWindow.HideDirectImage();
        
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
