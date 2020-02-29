using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterIcon : MonoBehaviour
{
    private Disaster disaster;

    public void SetDisaster(Disaster d) { disaster = d; }
    public Disaster GetDisaster() { return disaster; }
    
}
