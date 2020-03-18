using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeToggle : ToggleButton
{
    [SerializeField]
    [TextArea(3,10)]
    private string description = "";

    public string GetDescription() { return description; }
}
