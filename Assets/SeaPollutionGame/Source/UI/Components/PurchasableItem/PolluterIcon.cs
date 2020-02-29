using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PolluterIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject targetPolluter = null;

    [SerializeField]
    private TextMeshProUGUI targetText = null;
    
    private GameObject spaceForPolluter = null;

    private PolluterAttrib polluterAttrib = null;

    private GameObject polluterDragged = null;

    private bool beingDragged = false;

    public void SetSpace(GameObject s) { spaceForPolluter = s; }

    public void SetPolluterAttributes(PolluterAttrib attrib)
    {
        polluterAttrib = attrib;
    }
    
    public Polluter GetPolluter() { return targetPolluter.GetComponentInChildren<Polluter>(); }

    public void SetText(string s) { targetText.text = s; }

    private void Update()
    {
        transform.position = Input.mousePosition;

        if(polluterDragged != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(transform.position);
            pos.y = spaceForPolluter.transform.position.y;
            polluterDragged.transform.position = pos;
        } else
        {
            Debug.LogError("[PolluterIcon] Update: no target polluter instantied");
        }
        
    }

    public GameObject InstantiatePolluter()
    {
        polluterDragged = Instantiate(targetPolluter, spaceForPolluter.transform);

        Polluter p = polluterDragged.GetComponentInChildren<Polluter>();

        p.SetAttrib(polluterAttrib);

        var drop = polluterDragged.AddComponent<Drop>();
        drop.SetOriginalPos(transform.parent, transform.localPosition);

        drop.OnInvalidSpace += Drop_OnInvalidSpace;
        drop.OnValidSpace += Drop_OnValidSpace;

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
}
