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

    [SerializeField]
    private AttribLoader attribLoader = null;

    [SerializeField]
    private PlayerController playerController = null;

    [SerializeField]
    private RectTransform factoriesMenu = null;
    [SerializeField]
    private RectTransform filtersMenu = null;

    [SerializeField]
    private Button factoriesButton = null;
    [SerializeField]
    private Button filtersButton = null;

    [SerializeField]
    private GameObject spaceFactories = null;
    [SerializeField]
    private GameObject spaceFilters = null;

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
        ShowFactories();

        var factoryAttribs = purchasables[0].polluterAttribs;
        foreach (var factoryAttrib in attribLoader.attribData.factoryList)
        {
            factoryAttribs.Add(factoryAttrib);
        }
        var filterAttribs = purchasables[1].polluterAttribs;
        foreach (var filterAttrib in attribLoader.attribData.filterList)
        {
            filterAttribs.Add(filterAttrib);
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
                    purchasableIcon.SetSpace(spaceFactories);
                }

                if(polluter is Filter)
                {
                    purchasableIcon = Instantiate(pur.purchasableIcon, filtersMenu);
                    purchasableIcon.SetSpace(spaceFilters);
                }

                purchasableIcon.SetPolluterAttributes(pur.polluterAttribs[i]);
                purchasableIcon.playerController = playerController;

                purchasableIcon.SetText((i + 1).ToString());
            }
        }
        
    }

    public void ShowFactories()
    {
        factoriesButton.onClick.RemoveListener(ShowFactories);
        factoriesButton.interactable = false;

        HideFilters();

        factoriesMenu.gameObject.SetActive(true);

        filtersButton.interactable = true;
        filtersButton.onClick.AddListener(ShowFilters);
    }

    public void ShowFilters()
    {
        filtersButton.onClick.RemoveListener(ShowFilters);
        filtersButton.interactable = false;

        HideFactories();

        filtersMenu.gameObject.SetActive(true);

        factoriesButton.interactable = true;
        factoriesButton.onClick.AddListener(ShowFactories);
    }

    public void HideFactories()
    {
        factoriesMenu.gameObject.SetActive(false);
    }

    public void HideFilters()
    {
        filtersMenu.gameObject.SetActive(false);
    }
}
