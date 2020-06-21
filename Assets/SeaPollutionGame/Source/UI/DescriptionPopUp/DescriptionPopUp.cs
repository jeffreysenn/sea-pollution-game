using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField]
    private WorldWindow worldWindow = null;
    [SerializeField]
    private PolluterContent polluterContent = null;
    [SerializeField]
    private ContextualPolluterContent contextualPolluterContent = null;
    [SerializeField]
    private NodeContent nodeContent = null;
    [SerializeField]
    private BalticContent balticContent = null;
    [SerializeField]
    private string balticTag = "Sea";
    [SerializeField]
    private DisasterContent disasterContent = null;
    [SerializeField]
    private TutorialContent tutorialContent = null;
    [SerializeField]
    private GoalContent goalContent = null;
    [SerializeField]
    private ModeContent modeContent = null;

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
    public string GetBlockingTag() { return blockingRaycastTag; }
    private bool isBlocked = false;

    [Header("Contextual Anchors")]
    [SerializeField]
    private ScreenArea screenArea = null;
    [SerializeField]
    private RectTransform anchorTop = null;
    [SerializeField]
    private RectTransform anchorBot = null;
    [SerializeField]
    private RectTransform anchorLeft = null;
    [SerializeField]
    private RectTransform anchorRight = null;
    [SerializeField]
    private float positionOffset = 30f;
    private ScreenPosition currentScreenPosition = ScreenPosition.MIDDLE;
    
    private Vector2 anchorTopVector = new Vector2(0.5f, 0);
    private Vector2 anchorBotVector = new Vector2(0.5f, 1);
    private Vector2 anchorLeftVector = new Vector2(1, 0.5f);
    private Vector2 anchorRightVector = new Vector2(0, 0.5f);

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip popUpOpen = null;


    private List<PopUpContent> allPopupContents = new List<PopUpContent>();

    //private bool currentFromGame = false;
    private GameObject currentGameObject = null;
    private PopUpContent currentShownContent = null;

    GraphicRaycaster graphicRaycaster = null;
    PointerEventData pointerEventData = null;
    EventSystem eventSystem = null;

    private PlayerController playerController = null;
    private WorldStateManager worldStateManager = null;

    private void Start()
    {
        playerController = UIManager.Instance.playerController;
        worldStateManager = UIManager.Instance.worldStateManager;

        graphicRaycaster = GetComponent<GraphicRaycaster>();
        if(graphicRaycaster == null) { Debug.LogError("[DescriptionPopUp] Start: no GraphicRaycaster found"); }
        eventSystem = EventSystem.current;
        if (eventSystem == null) { Debug.LogError("[DescriptionPopUp] Start: no EventSystem found"); }

        allPopupContents.Add(polluterContent);
        allPopupContents.Add(nodeContent);
        allPopupContents.Add(balticContent);
        allPopupContents.Add(disasterContent);
        allPopupContents.Add(tutorialContent);
        allPopupContents.Add(goalContent);
        allPopupContents.Add(modeContent);

        foreach(PopUpContent puc in allPopupContents)
        {
            puc.parentPopUp = this;
            HideDirectPopup(puc);
        }

        //specific to PolluterContent
        polluterContent.worldWindow = worldWindow;

        //specific to ContextualPolluterContent
        contextualPolluterContent.parentPopUp = this;
        contextualPolluterContent.HideDirectPopup();
    }
    
    private void Update()
    {
        transform.position = Input.mousePosition;
        currentScreenPosition = screenArea.GetPosition(Input.mousePosition);

        bool uiRaycast = UIRaycasting();
        bool igRaycast = InGameRaycasting();
        
        if(!uiRaycast && !igRaycast)
        {
            currentGameObject = null;

            if (currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }
        }

        // specific to ContextualPolluterContent (right click to show, left click outside to close)
        if (Input.GetButtonDown("Fire2"))
        {
            if (contextualPolluterContent.isShown)
                contextualPolluterContent.HideDirectPopup();

            Space space = playerController.GetMouseHitComp<Space>();
            if(space != null)
            {
                Polluter polluter = space.GetPolluter();

                if(polluter != null)
                {
                    if(polluter.GetOwnerID() == worldStateManager.GetCurrentPlayerID())
                    {
                        if (contextualPolluterContent.CheckPolluter(polluter, space))
                        {
                            if (currentShownContent != null)
                                ShowPopup(contextualPolluterContent, false, true, ScreenPositionUtils.GetOppositePosition(currentShownContent.currentAnchor, true, false));
                            else
                                ShowPopup(contextualPolluterContent, false);
                        }
                    }
                }
            }
        }

        if(Input.GetButtonDown("Fire1") && contextualPolluterContent.isShown)
        {
            //check outside of content itself
            bool isInsideContent = false;

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> graphicResults = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, graphicResults);

            EventSystem.current.RaycastAll(pointerEventData, graphicResults);

            foreach (RaycastResult rr in graphicResults)
            {
                ContextualPolluterContent temp = rr.gameObject.GetComponentInChildren<ContextualPolluterContent>();
                if(temp != null)
                {
                    isInsideContent = true;
                    break;
                }
            }

            if(!isInsideContent)
            {
                contextualPolluterContent.HideDirectPopup();
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

            TutorialArea tutorialArea = rr.gameObject.GetComponentInChildren<TutorialArea>();
            if(tutorialArea != null)
            {
                hasHit = true;

                if(tutorialArea.gameObject != currentGameObject)
                {
                    currentGameObject = tutorialArea.gameObject;

                    if(tutorialContent.CheckGraphicTutorial(tutorialArea))
                    {
                        HidePopupOtherThan(tutorialContent);
                        ShowPopup(tutorialContent);
                    }
                }
            }

            /*
            PolluterIcon polluterIcon = rr.gameObject.GetComponentInChildren<PolluterIcon>();
            if(polluterIcon != null)
            {
                hasHit = true;

                if(polluterIcon.gameObject != currentGameObject)
                {
                    currentGameObject = polluterIcon.gameObject;

                    if(polluterContent.CheckGraphicPolluter(polluterIcon))
                    {
                    }
                }
            }
            */

            PurchasableIcon purchasableIcon = rr.gameObject.GetComponentInChildren<PurchasableIcon>();
            if (purchasableIcon != null)
            {
                hasHit = true;

                if (purchasableIcon.gameObject != currentGameObject)
                {
                    currentGameObject = purchasableIcon.gameObject;

                    if (polluterContent.CheckGraphicPolluter(purchasableIcon))
                    {
                        HidePopupOtherThan(polluterContent);
                        ShowPopup(polluterContent);
                    }
                }
            }

            GoalItem goalItem = rr.gameObject.GetComponentInChildren<GoalItem>();
            if(goalItem != null)
            {
                hasHit = true;

                if (goalItem.gameObject != currentGameObject)
                {
                    currentGameObject = goalItem.gameObject;

                    if (goalContent.CheckGraphicGoal(goalItem))
                    {
                        HidePopupOtherThan(goalContent);
                        ShowPopup(goalContent);
                    }
                }
            }

            ModeToggle modeToggle = rr.gameObject.GetComponentInChildren<ModeToggle>();
            if(modeToggle != null)
            {
                hasHit = true;

                if (modeToggle.gameObject != currentGameObject)
                {
                    currentGameObject = modeToggle.gameObject;

                    if (modeContent.CheckGraphicMode(modeToggle))
                    {
                        HidePopupOtherThan(modeContent);
                        ShowPopup(modeContent);
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

                        if (disasterContent.CheckGraphicDisaster(disasterIcon))
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
        if (isBlocked) return false;

        bool hasHit = false;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<DrawDescription>() != null)
            {
                hasHit = true;

                if (hit.transform.gameObject != currentGameObject)
                {
                    currentGameObject = hit.transform.gameObject;
                    
                    if (polluterContent.CheckPolluter(currentGameObject.GetComponentInChildren<Polluter>()) && raycastPolluter)
                    {
                        HidePopupOtherThan(polluterContent);
                        ShowPopup(polluterContent);
                    }
                    else if (balticContent.CheckBalticSea(currentGameObject.GetComponentInChildren<Node>(), balticTag) && raycastBaltic)
                    {
                        HidePopupOtherThan(balticContent);
                        ShowPopup(balticContent);
                    }
                    else if (nodeContent.CheckNode(currentGameObject.GetComponentInChildren<Node>()) && raycastNode)
                    {
                        HidePopupOtherThan(nodeContent);
                        ShowPopup(nodeContent);
                    }
                    else if (nodeContent.CheckFlow(currentGameObject.GetComponentInChildren<Flow>()) && raycastFlow)
                    {
                        HidePopupOtherThan(nodeContent);
                        ShowPopup(nodeContent);
                    }
                }
            }
        }
        
        return hasHit;
    }

    private void ShowPopup(PopUpContent content, bool save = true, bool useSpecificPosition = false, ScreenPosition specificPosition = ScreenPosition.MIDDLE)
    {
        if (playerController.GetState() == PlayerController.State.HOLDING) return;

        if(save) currentShownContent = content;

        // set position depending on screen area
        RectTransform rt = content.canvas.GetComponent<RectTransform>();

        if (currentScreenPosition == ScreenPosition.MIDDLE)
        {
            currentScreenPosition = content.defaultAnchor;
        }

        ScreenPosition screenPosition = ScreenPosition.MIDDLE;

        if(useSpecificPosition)
        {
            screenPosition = specificPosition;
        } else
        {
            screenPosition = currentScreenPosition;
        }

        Vector2 newPosition = Vector2.zero;

        switch (screenPosition)
        {
            case ScreenPosition.TOP:
            rt.anchorMax = anchorBotVector;
            rt.anchorMin = anchorBotVector;
            rt.pivot = anchorBotVector;
            newPosition = new Vector2(0, -positionOffset);
            break;
            case ScreenPosition.BOTTOM:
            rt.anchorMax = anchorTopVector;
            rt.anchorMin = anchorTopVector;
            rt.pivot = anchorTopVector;
            newPosition = new Vector2(0, positionOffset);
            break;
            case ScreenPosition.LEFT:
            rt.anchorMax = anchorRightVector;
            rt.anchorMin = anchorRightVector;
            rt.pivot = anchorRightVector;
            newPosition = new Vector2(positionOffset, 0);
            break;
            case ScreenPosition.RIGHT:
            rt.anchorMax = anchorLeftVector;
            rt.anchorMin = anchorLeftVector;
            rt.pivot = anchorLeftVector;
            newPosition = new Vector2(-positionOffset, 0);
            break;
            default:
            break;
        }

        rt.anchoredPosition = newPosition;
        content.currentAnchor = screenPosition;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        
        content.ShowPopup();

        if (content.imageToShow)
        {
            worldWindow.ShowImage(content.imageIsDisaster);
        }
        else
        {
            worldWindow.HideImage();
        }


        audioSource.PlayOneShot(popUpOpen);
    }

    private void HidePopup(PopUpContent content)
    {
        currentShownContent = null;
        
        if (content.isShown)
        {
            worldWindow.HideImage();
        }

        content.HidePopup();
    }

    private void HideDirectPopup(PopUpContent content)
    {
        currentShownContent = null;

        content.HideDirectPopup();

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

    public void FeedbackNotEnoughCoins()
    {
        polluterContent.FeedbackCoins();
    }

    public void FeedbackNotEnoughRemovalCoins()
    {
        polluterContent.FeedbackRemovalCoins();
    }

}

/*
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
            currentGameObject = null;

            if(currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }
        }

        currentScreenPosition = screenArea.GetPosition(Input.mousePosition);
        

        bool uiRaycast = UIRaycasting();

        if(uiRaycast)
        {
            currentFromGame = false;
            transform.position = Input.mousePosition;
        }
    

        bool igRaycast = false;

        if (Input.GetButtonDown("Fire1") && !isBlocked && !uiRaycast)
        {
            if (currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }

            igRaycast = InGameRaycasting();

            if (!igRaycast)
            {
                currentGameObject = null;

                if (currentShownContent != null)
                {
                    HidePopup(currentShownContent);
                }
            }
            else
            {
                currentFromGame = true;
                transform.position = Camera.main.WorldToScreenPoint(currentGameObject.transform.position);
            }
        }

        // cleanup
        if (!uiRaycast && !currentFromGame)
        {
            currentGameObject = null;

            if (currentShownContent != null)
            {
                HidePopup(currentShownContent);
            }
        }
*/