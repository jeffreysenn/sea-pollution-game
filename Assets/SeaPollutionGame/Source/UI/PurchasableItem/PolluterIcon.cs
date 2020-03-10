using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PolluterIcon : MonoBehaviour, IPointerClickHandler
{
    /*
     * PolluterIcon:
     *  OnClick: Creates the target Polluter if the space is correct
     *  Fire2: Remove the GameObject (cancel)
     */

    [SerializeField]
    private Polluter targetPolluter = null;

    [SerializeField]
    private TextMeshProUGUI targetText = null;
    
    private GameObject spaceForPolluter = null;

    private PolluterAttrib polluterAttrib = null;

    private GameObject gameObjectDragged = null;
    private Polluter polluterDragged = null;

    public int polluterId { get; set; }

    private bool beingDragged = false;

    public void SetSpace(GameObject s) { spaceForPolluter = s; }

    public PlayerController playerController { get; set; }

    public void SetPolluterAttributes(PolluterAttrib attrib)
    {
        polluterAttrib = attrib;
    }
    
    public Polluter GetPolluter() { return targetPolluter; }

    public void SetText(string s) { targetText.text = s; }

    private void Start()
    {
        int id = UIManager.Instance.worldStateManager.GetCurrentPlayerID();

        // spaces
        Space[] spaces = UIManager.Instance.spaceManager.spaces;
        foreach (Space s in spaces)
        {
            if(s.CanPlacePolluter(id, polluterAttrib))
            {
                s.Highlight();
            }
        }

        // flows
        UIManager.Instance.flowManager.Show();
    }

    private void OnDestroy()
    {
        Space[] spaces = UIManager.Instance.spaceManager.spaces;
        foreach (Space s in spaces)
        {
            s.HideHighlight();
        }

        UIManager.Instance.flowManager.Hide();
    }

    private void Update()
    {
        transform.position = Input.mousePosition;

        if(Input.GetButtonDown("Fire2")) {
            Destroy(gameObject);
        }
    }

    public GameObject InstantiatePolluter()
    {
        GameObject g = Instantiate(targetPolluter.gameObject, spaceForPolluter.transform);

        Polluter p = g.GetComponentInChildren<Polluter>();
        
        p.SetAttrib(polluterAttrib);
        p.polluterId = polluterId;

        polluterDragged = p;
        gameObjectDragged = g;

        return g;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InstantiatePolluter();

        playerController.Hold(polluterDragged);

        bool dropped = playerController.TryDrop();

        if(dropped)
        {
            Destroy(gameObject);
        } else
        {
            playerController.CancelHold();
        }
    }
}
