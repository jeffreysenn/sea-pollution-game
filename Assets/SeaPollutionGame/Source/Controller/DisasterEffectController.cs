using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterEffectController : MonoBehaviour
{
    [System.Serializable]
    class DisasterEffect
    {
        public string title;
        public List<GameObject> sound;
        public List<GameObject> lights;
        public List<GameObject> fx;
    }

    [SerializeField]
    private List<DisasterEffect> disasterEffects = null;

    private DisasterManager disasterManager = null;

    private void Start()
    {
        disasterManager = FindObjectOfType<DisasterManager>();

        DisableAllEffects();

        disasterManager.AddDisasterEventListener(OnDisaster);
        disasterManager.AddNoDisasterEventListener(OnNoDisaster);
    }

    void OnDisaster(Disaster d)
    {
        DisableAllEffects();

        foreach(DisasterEffect de in disasterEffects)
        {
            if(de.title == d.title)
            {
                foreach (GameObject g in de.sound)
                    g.SetActive(true);

                foreach (GameObject g in de.lights)
                    g.SetActive(true);

                foreach (GameObject g in de.fx)
                {
                    g.SetActive(true);
                    foreach(ParticleSystem ps in g.GetComponentsInChildren<ParticleSystem>())
                    {
                        ps.Play();
                    }
                }

                break;
            }
        }
    }

    void OnNoDisaster()
    {
        DisableAllEffects();
    }

    void DisableAllEffects()
    {
        foreach (DisasterEffect de in disasterEffects)
        {
            foreach (GameObject g in de.sound)
                g.SetActive(false);

            foreach (GameObject g in de.lights)
                g.SetActive(false);

            foreach (GameObject g in de.fx)
                g.SetActive(false);
        }
    }
}
