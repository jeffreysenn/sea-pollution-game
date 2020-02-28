using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PurchasableIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI targetText = null;

    [SerializeField]
    private PolluterIcon targetPolluterIcon = null;

    private Space spaceForPolluter = null;

    private PolluterAttrib polluterAttrib = null;

    WorldStateManager worldStateManager = null;

    private void Start()
    {
        worldStateManager = WorldStateManager.FindWorldStateManager();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameObject.activeInHierarchy) return;

        if (worldStateManager.GetMoney(worldStateManager.GetCurrentPlayerID()) < polluterAttrib.economicAttrib.price) return;

        PolluterIcon newIcon = Instantiate(targetPolluterIcon, transform);
        newIcon.transform.position = Input.mousePosition;

        newIcon.SetPolluterAttributes(polluterAttrib);
        newIcon.SetSpace(spaceForPolluter);
        newIcon.SetText(targetText.text);

        newIcon.InstantiatePolluter();
    }


    public void SetSpace(Space s) { spaceForPolluter = s; }

    public void SetPolluterAttributes(PolluterAttrib attrib){ polluterAttrib = attrib; }

    public void SetText(string s) { targetText.text = s; }

    public PolluterIcon GetPolluterIcon() { return targetPolluterIcon; }
}
