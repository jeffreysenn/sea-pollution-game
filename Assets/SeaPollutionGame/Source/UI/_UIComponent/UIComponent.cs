using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIComponent : MonoBehaviour
{
    private bool _isShown = false;
    public bool isShown { get { return _isShown; } }

    public virtual void Show()
    {

    }

    public virtual void Hide()
    {

    }

    public virtual void HideDirect()
    {

    }
}
