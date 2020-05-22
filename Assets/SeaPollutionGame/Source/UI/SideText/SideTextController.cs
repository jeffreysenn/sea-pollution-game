using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SideTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title = null;
    [SerializeField]
    private TextMeshProUGUI description = null;

    private string currentTitle = "";
    private string currentDescription = "";

    public void SetText(string s = "", string d = "")
    {
        currentTitle = s;
        title.text = s;

        currentDescription = d;
        description.text = d;
    }

    public void SetTemporaryText(string s = "", string d = "")
    {
        title.text = s;
        description.text = d;
    }

    public void ResetTemporaryText()
    {
        SetText(currentTitle, currentDescription);
    }
}
