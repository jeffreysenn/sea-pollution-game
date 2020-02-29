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

    private GameObject polluterDragged = null;

    private bool beingDragged = false;

    public void SetSpace(GameObject s) { spaceForPolluter = s; }

    public PlayerController playerController { get; set; }

    public void SetPolluterAttributes(PolluterAttrib attrib)
    {
        polluterAttrib = attrib;
    }
    
    public Polluter GetPolluter() { return targetPolluter; }

    public void SetText(string s) { targetText.text = s; }

    private void Update()
    {
        transform.position = Input.mousePosition;

        if(Input.GetButtonDown("Fire2")) { Destroy(gameObject); }
    }

    public GameObject InstantiatePolluter()
    {
        polluterDragged = Instantiate(targetPolluter.gameObject, spaceForPolluter.transform);

        Polluter polluter = polluterDragged.GetComponentInChildren<Polluter>();

        polluter.SetAttrib(polluterAttrib);

        return polluterDragged;
    }

    private void Drop_OnValidSpace(Drop drop)
    {
        drop.OnValidSpace -= Drop_OnValidSpace;
        
        Destroy(gameObject);
    }

    private void Drop_OnInvalidSpace(Drop drop)
    {
        drop.OnInvalidSpace -= Drop_OnInvalidSpace;

        Destroy(polluterDragged);
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject instantiatedPolluter = InstantiatePolluter();

        playerController.Hold(instantiatedPolluter.GetComponentInChildren<Polluter>());

        bool dropped = playerController.TryDrop();

        if(dropped)
        {
            Destroy(gameObject);
        } else
        {
            playerController.CancelHold();
            Destroy(instantiatedPolluter);
        }

        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray); //(transform.position, new Vector3(0, -1, 0));
        Space validSpace = null;
        

        foreach (var hit in hits)
        {
            var hitObj = hit.transform.gameObject;

            if(targetPolluter.GetComponentInChildren<Factory>() != null)
            {
                validSpace = hitObj.GetComponent<FactorySpace>();
            }

            if(targetPolluter.GetComponentInChildren<Filter>() != null)
            {
                validSpace = hitObj.GetComponent<FilterSpace>();
            }

            if (validSpace)
            {
                break;
            }
        }

        if (validSpace && validSpace.ownerID == 
            WorldStateManager.FindWorldStateManager().GetCurrentPlayerID())
        {
            GameObject instantiatedPolluter = InstantiatePolluter(validSpace);
            

            Destroy(gameObject);
        }
        */
    }
}
