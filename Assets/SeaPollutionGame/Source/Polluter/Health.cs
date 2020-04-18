using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Health : MonoBehaviour
{
    public event Action<Disaster> OnDeathFrom;
    float hp = 100;

    public void AddHealth(float dh, Disaster from = null) { 
        hp += dh; 
        if(hp > 100) { hp = 100; }
        else if(hp <= 0) {
            OnDeathFrom?.Invoke(from);
        }
    }

    public float GetHealth() { return hp; }

    public bool IsAlive() { return hp > 0; }
}
