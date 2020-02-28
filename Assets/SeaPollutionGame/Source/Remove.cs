using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            var polluter = GetComponent<Polluter>();
            if(polluter.CanRemove())
            {
                Destroy(polluter.gameObject);
            }
        }    
    }
}
