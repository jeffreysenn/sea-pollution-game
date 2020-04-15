using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PolluterIcon : MonoBehaviour //, IPointerClickHandler
{
    public event Action OnDrag;
    public event Action OnRelease;

    [SerializeField]
    private bool isInteractible = true;

    [SerializeField]
    private Polluter targetPolluter = null;

    [SerializeField]
    private TextMeshProUGUI targetText = null;
    
    private GameObject spaceForPolluter = null;

    private PolluterAttrib polluterAttrib = null;

    private GameObject gameObjectDragged = null;
    private Polluter polluterDragged = null;

    public int polluterId { get; set; }

    public void SetSpace(GameObject s) { spaceForPolluter = s; }

    public PlayerController playerController { get; set; }

    public void SetPolluterAttributes(PolluterAttrib attrib)
    {
        polluterAttrib = attrib;
    }
    public PolluterAttrib GetPolluterAttributes() { return polluterAttrib; }
    
    public Polluter GetPolluter() { return targetPolluter; }

    public void SetText(string s) { targetText.text = s; }

    private void Start()
    {
        if(isInteractible)
        {
            OnDrag?.Invoke();

            playerController.Hold();
        }
    }

    private void OnDestroy()
    {
        OnRelease?.Invoke();
    }

    private void Update()
    {
        if(isInteractible)
        {
            transform.position = Input.mousePosition;

            /*
            if(Input.GetButtonDown("Fire2")) {
                playerController.CancelHold();
                Destroy(gameObject);
            }
            */

            if (Input.GetButtonUp("Fire1"))
            {
                InstantiatePolluter();

                playerController.Hold(polluterDragged);

                bool dropped = playerController.TryDrop();

                if (dropped)
                {
                    Destroy(gameObject);
                }
                else
                {
                    playerController.CancelHold();
                    Destroy(gameObject);
                }
            }
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
