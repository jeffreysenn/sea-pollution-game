using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct PurchasableItem
{
    public PurchasableIcon purchasableIcon;
    public List<PolluterAttrib> polluterAttribs;
}

public class PurchaseMenuController : MonoBehaviour
{
    /*
     * PurchaseMenuController: 
     *  Creates PurchasableIcon in the corresponding menu, with attributes
     *  Controls the display of each menu
     */
     
    private AttribLoader attribLoader = null;
    
    private PlayerController playerController = null;

    [SerializeField]
    private RectTransform factoriesMenu = null;
    [SerializeField]
    private ScrollRect factoriesScrollRect = null;

    [SerializeField]
    private RectTransform filtersMenu = null;
    [SerializeField]
    private ScrollRect filtersScrollRect = null;

    [SerializeField]
    private RectTransform recyclersMenu = null;
    [SerializeField]
    private ScrollRect recyclersScrollRect = null;

    [Header("Button")]
    [SerializeField]
    private Button factoriesButton = null;
    [SerializeField]
    private Button filtersButton = null;
    [SerializeField]
    private Button recyclersButton = null;

    [Header("Misc")]
    [SerializeField]
    private GameObject temporarySpace = null;

    public List<PurchasableItem> purchasables = new List<PurchasableItem> { };

    Polluter InstantiatePolluter(PolluterIcon polluter)
    {
        var clone = Instantiate(polluter, transform);
        //var drawDescription = clone.GetComponent<DrawPolluterDescription>();
        //drawDescription.descriptionText = descriptionText;
        return clone.GetComponent<Polluter>();
    }

    void Start()
    {
        attribLoader = UIManager.Instance.attribLoader;
        playerController = UIManager.Instance.playerController;
        var data = attribLoader.LoadLazy();

        ShowFactories();

        var factoryAttribs = purchasables[0].polluterAttribs;
        foreach (var factoryAttrib in data.factoryList)
        {
            factoryAttribs.Add(factoryAttrib);
        }
        var filterAttribs = purchasables[1].polluterAttribs;
        foreach (var filterAttrib in data.filterList)
        {
            filterAttribs.Add(filterAttrib);
        }
        var recyclerAttribs = purchasables[2].polluterAttribs;
        foreach(var recyclerAttrib in data.recyclerList)
        {
            recyclerAttribs.Add(recyclerAttrib);
        }
        
        
        foreach (var pur in purchasables)
        {
            for (int i = 0; i < pur.polluterAttribs.Count; i++)
            {
                PurchasableIcon purchasableIcon = null;
                Polluter polluter = pur.purchasableIcon.GetPolluterIcon().GetPolluter();

                if(polluter is Factory)
                {
                    purchasableIcon = Instantiate(pur.purchasableIcon, factoriesMenu);
                    purchasableIcon.SetSpace(temporarySpace);
                }

                if(polluter is Filter)
                {
                    purchasableIcon = Instantiate(pur.purchasableIcon, filtersMenu);
                    purchasableIcon.SetSpace(temporarySpace);
                }

                if(polluter is Recycler)
                {
                    purchasableIcon = Instantiate(pur.purchasableIcon, recyclersMenu);
                    purchasableIcon.SetSpace(temporarySpace);
                }

                purchasableIcon.SetPolluterAttributes(pur.polluterAttribs[i]);
                purchasableIcon.playerController = playerController;

                purchasableIcon.shopTransform = transform;

                purchasableIcon.polluterId = (i + 1);
                purchasableIcon.SetText(purchasableIcon.polluterId.ToString());

                purchasableIcon.OnBuy += PurchasableIcon_OnBuy;
            }
        }
        
    }

    private void PurchasableIcon_OnBuy(PurchasableIcon icon, bool success)
    {
        if(!success)
        {
            Feedback.Instance.FeedbackInsufficientCoins();
        }
    }

    public void ShowFactories()
    {
        HideFilters();
        HideRecyclers();

        factoriesButton.onClick.RemoveListener(ShowFactories);
        factoriesButton.interactable = false;
        factoriesScrollRect.gameObject.SetActive(true);
    }

    public void ShowFilters()
    {
        HideFactories();
        HideRecyclers();

        filtersButton.onClick.RemoveListener(ShowFilters);
        filtersButton.interactable = false;
        filtersScrollRect.gameObject.SetActive(true);
    }

    public void ShowRecyclers()
    {
        HideFactories();
        HideFilters();

        recyclersButton.onClick.RemoveListener(HideRecyclers);
        recyclersButton.interactable = false;
        recyclersScrollRect.gameObject.SetActive(true);
    }

    public void HideFactories()
    {
        //factoriesMenu.gameObject.SetActive(false);
        factoriesScrollRect.gameObject.SetActive(false);

        factoriesButton.interactable = true;
        factoriesButton.onClick.AddListener(ShowFactories);
    }

    public void HideFilters()
    {
        //filtersMenu.gameObject.SetActive(false);
        filtersScrollRect.gameObject.SetActive(false);

        filtersButton.interactable = true;
        filtersButton.onClick.AddListener(ShowFilters);
    }

    public void HideRecyclers()
    {
        recyclersScrollRect.gameObject.SetActive(false);

        recyclersButton.interactable = true;
        recyclersButton.onClick.AddListener(ShowRecyclers);
    }
}
