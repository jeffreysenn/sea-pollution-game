using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    private Button button = null;

    [SerializeField]
    private bool _toggled = false;
    public bool toggled { get; set; }

    public event Action<ToggleButton, bool> OnToggle;

    private void Start()
    {
        button.onClick.AddListener(OnClick);

        if(toggled)
        {
            ChangeGraphic();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        toggled = !toggled;

        ChangeGraphic();

        OnToggle?.Invoke(this, toggled);
    }

    protected virtual void ChangeGraphic()
    {
        if (toggled)
        {
            button.targetGraphic.color = button.colors.pressedColor;
        }
        else
        {
            button.targetGraphic.color = button.colors.normalColor;
        }
    }
}
