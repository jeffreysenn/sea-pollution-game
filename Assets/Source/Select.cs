using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gameObject.AddComponent<FollowMouse>();
            gameObject.AddComponent<Drop>();
            Destroy(this);
        }       
    }
}
