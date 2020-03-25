using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PurchasableIcon : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler //, IPointerClickHandler
{
    /*
     * PurchasableIcon:
     *  OnClick: Creates the corresponding PolluterIcon with attributes
     */

    [SerializeField]
    private TextMeshProUGUI targetText = null;

    [SerializeField]
    private PolluterIcon targetPolluterIcon = null;

    private GameObject spaceForPolluter = null;

    private PolluterAttrib polluterAttrib = null;

    WorldStateManager worldStateManager = null;

    public PlayerController playerController { get; set; }

    public Transform shopTransform { get; set; }

    public int polluterId { get; set; }

    private bool isDragging = false;

    public event Action<PurchasableIcon, bool> OnBuy;

    private void Start()
    {
        worldStateManager = FindObjectOfType<WorldStateManager>();
    }

    private void InstantiatePolluterIcon()
    {
        if (!gameObject.activeInHierarchy) return;

        if (worldStateManager.GetCurrentPlayerState().GetMoney() < polluterAttrib.economicAttrib.price)
        {
            OnBuy?.Invoke(this, false);
            return;
        }
        
        PolluterIcon newIcon = null;

        newIcon = Instantiate(targetPolluterIcon, shopTransform);

        newIcon.OnDrag += NewIcon_OnDrag;
        newIcon.OnRelease += NewIcon_OnRelease;

        newIcon.transform.position = Input.mousePosition;

        newIcon.SetPolluterAttributes(polluterAttrib);
        newIcon.SetSpace(spaceForPolluter);
        newIcon.SetText(targetText.text);
        newIcon.playerController = playerController;
        newIcon.polluterId = polluterId;

        OnBuy?.Invoke(this, true);
    }

    private void NewIcon_OnDrag()
    {
        ShowHighlight();
    }

    private void NewIcon_OnRelease()
    {
        HideHighlight();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        InstantiatePolluterIcon();
    }


    public void SetSpace(GameObject s) { spaceForPolluter = s; }

    public void SetPolluterAttributes(PolluterAttrib attrib) { polluterAttrib = attrib; }

    public PolluterAttrib GetPolluterAttributes() { return polluterAttrib; }

    public void SetText(string s) { targetText.text = s; }

    public PolluterIcon GetPolluterIcon() { return targetPolluterIcon; }

    public void OnPointerDown(PointerEventData eventData)
    {
        InstantiatePolluterIcon();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playerController.GetState() != PlayerController.State.HOLDING)
            ShowHighlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(playerController.GetState() != PlayerController.State.HOLDING)
            HideHighlight();
    }

    private void ShowHighlight(bool space = true, bool flow = true)
    {
        if(space)
        {
            int id = UIManager.Instance.worldStateManager.GetCurrentPlayerID();
            UIManager.Instance.spaceManager.HightlightAvailablePlaces(id, polluterAttrib);
        }

        if(flow)
            UIManager.Instance.flowManager.Show();
    }

    private void HideHighlight(bool space = true, bool flow = true)
    {
        if(space)
            UIManager.Instance.spaceManager.HideHighlight();

        if(flow)
            UIManager.Instance.flowManager.Hide();
    }
}
