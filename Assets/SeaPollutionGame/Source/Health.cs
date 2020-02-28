using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    UnityEvent deathEvent = new UnityEvent { };
    float hp = 100;

    public void AddHealth(float dh) { 
        hp += dh; 
        if(hp > 100) { hp = 100; }
        else if(hp <= 0) { deathEvent.Invoke(); }
    }

    public float GetHealth() { return hp; }

    public void AddDeathEventListener(UnityAction action) { deathEvent.AddListener(action); }

    public bool IsAlive() { return hp > 0; }
}
