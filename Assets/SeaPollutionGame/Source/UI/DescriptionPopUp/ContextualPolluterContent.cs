using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextualPolluterContent : PopUpContent
{
    [Header("Icons")]
    [SerializeField]
    private PolluterIcon factoryIcon = null;
    [SerializeField]
    private PolluterIcon filterIcon = null;
    [SerializeField]
    private PolluterIcon recyclerIcon = null;

    [Header("Buttons")]
    [SerializeField]
    private Button btnRepair = null;
    [SerializeField]
    private Button btnSell = null;
    [SerializeField]
    private Button btnCancel = null;

    [Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI txtRepair = null;
    [SerializeField]
    private string repairInitialText = "Repair";
    [SerializeField]
    private TextMeshProUGUI txtSell = null;
    [SerializeField]
    private string sellInitialText = "Sell";

    private PlayerController playerController = null;

    private Polluter currentPolluter = null;
    private Space currentSpace = null;

    private Vector3 startingPos = Vector3.zero;
    private RectTransform ownRectTransform = null;

    private string defaultTag = null;

    private void Start()
    {
        playerController = UIManager.Instance.playerController;
        ownRectTransform = GetComponent<RectTransform>();
        defaultTag = transform.tag;

        startingPos = ownRectTransform.anchoredPosition;

        factoryIcon.gameObject.SetActive(false);
        factoryIcon.SetInteractible(false);

        filterIcon.gameObject.SetActive(false);
        filterIcon.SetInteractible(false);

        recyclerIcon.gameObject.SetActive(false);
        recyclerIcon.SetInteractible(false);

        btnRepair.onClick.AddListener(BtnRepair_OnClick);
        btnSell.onClick.AddListener(BtnSell_OnClick);
        btnCancel.onClick.AddListener(BtnCancel_OnClick);
    }

    private void BtnRepair_OnClick()
    {
        if(currentPolluter != null)
        {
            if(playerController.TryRepair(currentPolluter))
            {
                HideDirectPopup();
            }
        }
    }

    private void BtnSell_OnClick()
    {
        if(currentPolluter != null)
        {
            if(playerController.TryRemove(currentSpace))
            {
                HideDirectPopup();
            }
        }
    }

    private void BtnCancel_OnClick()
    {
        HideDirectPopup();
    }
    
    public bool CheckPolluter(Polluter polluter, Space space)
    {
        if(polluter != null && space != null)
        {
            PolluterAttrib polluterAttrib = polluter.GetAttrib();

            //icons
            factoryIcon.gameObject.SetActive(false);
            filterIcon.gameObject.SetActive(false);
            recyclerIcon.gameObject.SetActive(false);

            if (polluterAttrib is FactoryAttrib)
            {
                factoryIcon.SetPolluterAttributes(polluterAttrib);
                factoryIcon.SetText(polluter.polluterId.ToString());
                factoryIcon.gameObject.SetActive(true);
            }

            if (polluterAttrib is FilterAttrib)
            {
                filterIcon.SetPolluterAttributes(polluterAttrib);
                filterIcon.SetText(polluter.polluterId.ToString());
                filterIcon.gameObject.SetActive(true);
            }

            if (polluterAttrib is RecyclerAttrib)
            {
                recyclerIcon.SetPolluterAttributes(polluterAttrib);
                recyclerIcon.SetText(polluter.polluterId.ToString());
                recyclerIcon.gameObject.SetActive(true);
            }

            currentPolluter = polluter;
            currentSpace = space;

            bool canRepair = playerController.CanRepair(polluter);
            btnRepair.interactable = canRepair;

            if (canRepair)
                txtRepair.text = repairInitialText +  " (-" + playerController.GetRepairCost(polluter).ToString() +")";
            else
                txtRepair.text = repairInitialText;

            txtSell.text = sellInitialText + " (-" + polluterAttrib.economicAttrib.removalCost + ")";

            return true;
        }

        return false;
    }

    public override void ShowPopup()
    {
        base.ShowPopup();

        transform.SetParent(parentPopUp.transform.parent);
        transform.SetSiblingIndex(parentPopUp.transform.GetSiblingIndex());

        transform.tag = parentPopUp.GetBlockingTag();

        canvas.blocksRaycasts = true;
    }

    public override void HidePopup(bool instant = false)
    {
        base.HidePopup(instant);

        transform.SetParent(parentPopUp.transform);
        transform.SetAsLastSibling();

        ownRectTransform.anchoredPosition = startingPos;

        transform.tag = defaultTag;

        canvas.blocksRaycasts = false;
    }
}
