using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LightComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject light = null;

    [SerializeField]
    private bool startToggle = false;

    private void Start()
    {
        light.SetActive(startToggle);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        light.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        light.SetActive(false);
    }
}
