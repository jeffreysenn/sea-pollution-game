using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Health : MonoBehaviour
{
    public event Action<Disaster> OnDeathFrom;
    public event Action<float> OnHealthModified;
    float hp = 100;

    public void AddHealth(float dh, Disaster from = null) { 
        hp += dh; 
        if(hp > 100) { hp = 100; }
        else if(hp <= 0) {
            hp = 0;
            OnDeathFrom?.Invoke(from);
        }
        OnHealthModified?.Invoke(hp);
    }

    public float GetHealth() { return hp; }

    public bool IsAlive() { return hp > 0; }

    public void Recover()
    {
        hp = 100;
        OnHealthModified?.Invoke(hp);
    }

    public float GetHealthPercent() { return GetHealth() / 100; }
}
