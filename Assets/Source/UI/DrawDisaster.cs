using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawDisaster : MonoBehaviour
{
    Text text = null;

    void Start()
    {
        text = GetComponent<Text>();
        var disasterManager = FindObjectsOfType<DisasterManager>()[0];
        disasterManager.AddDisasterEventListener(OnDisaster);
        disasterManager.AddNoDisasterEventListener(OnNoDisaster);
    }

    void OnDisaster(Disaster disaster)
    {
        text.text = disaster.title;
        text.color = Color.red;
    }

    void OnNoDisaster()
    {
        text.text = "No disaster";
        text.color = Color.green;
    }
}
