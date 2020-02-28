using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct PurchasableItem
{
    public GameObject polluter;
    public List<PolluterAttrib> polluterAttribs;
}

public class PurchaseMenuController : MonoBehaviour
{
    [SerializeField]
    private AttribLoader attribLoader = null;

    [SerializeField] Text descriptionText = null;

    [SerializeField]
    private RectTransform factoriesMenu = null;
    [SerializeField]
    private RectTransform filtersMenu = null;

    public List<PurchasableItem> purchasables = new List<PurchasableItem> { };

    Polluter InstantiatePolluter(GameObject obj)
    {
        var clone = Instantiate(obj, transform);
        var drawDescription = clone.GetComponent<DrawPolluterDescription>();
        drawDescription.descriptionText = descriptionText;
        return clone.GetComponent<Polluter>();
    }

    void Start()
    {
        // fetch data
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
            for (int i = 0; i != pur.polluterAttribs.Count; ++i)
            {
                var polluter = InstantiatePolluter(pur.polluter);
                polluter.SetAttrib(pur.polluterAttribs[i]);
            }
        }
    }
}
