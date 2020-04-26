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
    public bool toggled { get { return _toggled; } }

    [SerializeField]
    private Sprite offSprite = null;
    [SerializeField]
    private Sprite onSprite = null;
    [SerializeField]
    private GameObject glow = null;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip toggleOnClip = null;
    [SerializeField]
    private AudioClip toggleOffClip = null;

    public event Action<ToggleButton, bool> OnToggle;

    private void Start()
    {
        glow.SetActive(false);

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
        _toggled = !_toggled;

        ChangeGraphic();

        audioSource.Stop();
        if (_toggled)
        {
            if(toggleOnClip != null)
                audioSource.clip = toggleOnClip;
        } else
        {
            if(toggleOffClip != null)
                audioSource.clip = toggleOffClip;
        }
        audioSource.Play();

        OnToggle?.Invoke(this, toggled);
    }

    protected virtual void ChangeGraphic()
    {
        if (toggled)
        {
            button.image.overrideSprite = onSprite;
            //button.targetGraphic.color = button.colors.pressedColor;
            glow.SetActive(true);
        }
        else
        {
            button.image.overrideSprite = offSprite;
            //button.targetGraphic.color = button.colors.normalColor;
            glow.SetActive(false);
        }
    }

    public void Toggle()
    {
        _toggled = true;

        ChangeGraphic();
    }

    public void Untoggle()
    {
        _toggled = false;

        ChangeGraphic();
    }
}
