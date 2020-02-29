using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DisasterEvent : UnityEvent<Disaster> { }

public class DisasterManager : MonoBehaviour
{
    Disaster[] disasters = null;
    WorldStateManager stateManager = null;
    DisasterEvent disasterEvent = new DisasterEvent { };
    UnityEvent noDisasterEvent = new UnityEvent { };


    public void SetDisasters(Disaster[] arr)
    {
        disasters = new Disaster[arr.Length];
        System.Array.Copy(arr, disasters, arr.Length);
    }

    public void AddDisasterEventListener(UnityAction<Disaster> action) { disasterEvent.AddListener(action); }
    public void AddNoDisasterEventListener(UnityAction action) { noDisasterEvent.AddListener(action); }

    void Start()
    {
        stateManager = WorldStateManager.FindWorldStateManager();
        stateManager.AddEndTurnEventListener(GenDisaster);
    }

    void GenDisaster()
    {
        if (disasters != null)
        {
            var random = new System.Random();
            foreach (int i in Enumerable.Range(0, disasters.Length).OrderBy(x => random.Next()))
            {
                var disaster = disasters[i];
                float chance = Random.Range(0.0f, 1.0f);
                if (chance <= disaster.chancePerTurn)
                {
                    disasterEvent.Invoke(disaster);
                    OnDisaster(disaster);
                    return;
                }
            }
            noDisasterEvent.Invoke();
        }
    }


    void OnDisaster(Disaster disaster)
    {
        Health[] healthComps = FindObjectsOfType<Health>();
        foreach (var health in healthComps)
        {
            var polluter = health.GetComponent<Polluter>();
            var vulnerabilities = polluter.GetAttrib().vulnerabilityAttrib.vulnerabilities;
            if (vulnerabilities != null)
            {
                var vulnerability = System.Array.Find(vulnerabilities,
                    vul => { return vul.disasterName == disaster.title; });
                float damage = vulnerability.factor * disaster.damage;
                health.AddHealth(-damage);
            }
        }
    }

}
