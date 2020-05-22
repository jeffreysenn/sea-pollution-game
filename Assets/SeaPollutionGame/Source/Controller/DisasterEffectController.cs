using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterEffectController : MonoBehaviour
{
    [System.Serializable]
    class DisasterEffect
    {
        public string title;
        [Header("Sound")]
        public List<GameObject> sound;
        [Header("Lights")]
        public List<GameObject> lights;
        [Header("FX")]
        public List<GameObject> fx;
        [Range(0f, 100f)]
        public float minimalParticlePercentage = 0f;
    }

    class AudioSourceProcessed
    {
        public AudioSource audioSource;
        public float initialVolume;
    }

    class ParticleSystemProcessed
    {
        public ParticleSystem particleSystem;
        public float initialEmission;
    }

    [SerializeField]
    private List<DisasterEffect> disasterEffects = null;

    private DisasterManager disasterManager = null;

    [SerializeField]
    private float fadeInTimer = 0.5f;
    [SerializeField]
    private float fadeOutTimer = 1f;

    private CameraManager cameraManager = null;

    // current values
    private List<AudioSourceProcessed> audioSourceProcessed = new List<AudioSourceProcessed>();
    private List<ParticleSystemProcessed> particleSystemProcessed = new List<ParticleSystemProcessed>();

    private void Start()
    {
        disasterManager = FindObjectOfType<DisasterManager>();
        cameraManager = FindObjectOfType<CameraManager>();

        DisableAllEffects();

        disasterManager.AddDisasterEventListener(OnDisaster);
        disasterManager.AddNoDisasterEventListener(OnNoDisaster);

        cameraManager.OnStateChanged += CameraManager_OnStateChanged;
    }

    private void CameraManager_OnStateChanged(CameraManager.State state)
    {
        // fade out everything
        if (state == CameraManager.State.GAME)
        {
            StopAllCoroutines();
            ResetVolumes();
            ResetParticles();

            audioSourceProcessed = new List<AudioSourceProcessed>();
            particleSystemProcessed = new List<ParticleSystemProcessed>();

            foreach (DisasterEffect de in disasterEffects)
            {
                foreach(GameObject g in de.sound)
                {
                    StartCoroutine(FadeOutSound(g));
                }

                foreach(GameObject g in de.fx)
                {
                    StartCoroutine(FadeOutFX(g, de.minimalParticlePercentage));
                }

                foreach (GameObject g in de.lights)
                {
                    g.SetActive(false);
                }
            }
        }
    }

    void OnDisaster(Disaster d)
    {
        //DisableAllEffects();
        StopAllCoroutines();
        ResetVolumes();
        ResetParticles();

        audioSourceProcessed = new List<AudioSourceProcessed>();
        particleSystemProcessed = new List<ParticleSystemProcessed>();

        foreach(DisasterEffect de in disasterEffects)
        {
            if(de.title == d.title)
            {
                foreach (GameObject g in de.sound)
                {
                    g.SetActive(true);
                    StartCoroutine(FadeInSound(g));
                }

                foreach (GameObject g in de.lights)
                {
                    g.SetActive(true);
                }
                
                foreach (GameObject g in de.fx)
                {
                    g.SetActive(true);
                    StartCoroutine(FadeInFX(g));
                }

                break;
            }
        }
    }

    IEnumerator FadeInSound(GameObject go)
    {
        AudioSource[] audioSources = go.GetComponentsInChildren<AudioSource>();

        if (audioSources.Length > 0)
        {
            float[] initialVolumes = new float[audioSources.Length];

            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].Stop();
                audioSources[i].playOnAwake = false;

                initialVolumes[i] = audioSources[i].volume;

                AudioSourceProcessed asp = new AudioSourceProcessed();
                asp.audioSource = audioSources[i];
                asp.initialVolume = initialVolumes[i];
                audioSourceProcessed.Add(asp);

                audioSources[i].volume = 0;

                audioSources[i].Play();
            }

            float timer = 0f;

            while (timer < fadeInTimer)
            {
                timer += Time.deltaTime;

                for (int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].volume = Mathf.Lerp(0, initialVolumes[i], (timer / fadeInTimer));
                }

                yield return null;
            }
        }

        if(!disasterManager.transitionEnabled)
        {
            CameraManager_OnStateChanged(CameraManager.State.GAME);
        }
    }

    IEnumerator FadeOutSound(GameObject go)
    {
        if(go.activeInHierarchy)
        {
            AudioSource[] audioSources = go.GetComponentsInChildren<AudioSource>();

            if (audioSources.Length > 0)
            {
                float[] initialVolumes = new float[audioSources.Length];

                for (int i = 0; i < audioSources.Length; i++)
                {
                    audioSources[i].playOnAwake = false;

                    initialVolumes[i] = audioSources[i].volume;

                    AudioSourceProcessed asp = new AudioSourceProcessed();
                    asp.audioSource = audioSources[i];
                    asp.initialVolume = initialVolumes[i];
                    audioSourceProcessed.Add(asp);

                    audioSources[i].Play();
                }

                float timer = 0f;

                while (timer < fadeOutTimer)
                {
                    timer += Time.deltaTime;

                    for (int i = 0; i < audioSources.Length; i++)
                    {
                        audioSources[i].volume = Mathf.Lerp(initialVolumes[i], 0f, (timer / fadeOutTimer));
                    }

                    yield return null;
                }

                // reset values
                ResetVolumes();
            }
            
            go.SetActive(false);
        }
    }

    IEnumerator FadeInFX(GameObject go)
    {
        foreach (ParticleSystem ps in go.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }
        yield return null;
    }

    IEnumerator FadeOutFX(GameObject go, float minPercentage)
    {
        if(go.activeInHierarchy)
        {
            ParticleSystem[] particleSystems = go.GetComponentsInChildren<ParticleSystem>();

            ParticleSystem.EmissionModule[] em = new ParticleSystem.EmissionModule[particleSystems.Length];
            float[] initialEmissions = new float[particleSystems.Length];
            float[] minPercentageEmissions = new float[particleSystems.Length];

            for(int i = 0; i < particleSystems.Length; i++)
            {
                em[i] = particleSystems[i].emission;
                initialEmissions[i] = em[i].rateOverTime.constant;
                minPercentageEmissions[i] = initialEmissions[i] * (minPercentage / 100f); 

                ParticleSystemProcessed psp = new ParticleSystemProcessed();
                psp.particleSystem = particleSystems[i];
                psp.initialEmission = initialEmissions[i];
                particleSystemProcessed.Add(psp);
            }

            float timer = 0f;

            while (timer < fadeOutTimer)
            {
                timer += Time.deltaTime;

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    em[i].rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(initialEmissions[i], minPercentageEmissions[i], (timer / fadeOutTimer)));
                }

                yield return null;
            }

            if (minPercentage > 0f)
                yield break;
            
            bool allFinished = false;

            while (!allFinished)
            {
                bool finished = true;

                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i].particleCount > 0)
                    {
                        finished = false;
                        break;
                    }
                }

                allFinished = finished;

                yield return null;
            }
            
            
            // reset values
            ResetParticles();
        }
        
        go.SetActive(false);
    }

    void ResetVolumes()
    {
        foreach(AudioSourceProcessed asp in audioSourceProcessed)
        {
            asp.audioSource.Stop();
            asp.audioSource.volume = asp.initialVolume;
        }
    }

    void ResetParticles()
    {
        foreach(ParticleSystemProcessed psp in particleSystemProcessed)
        {
            psp.particleSystem.Stop();
            ParticleSystem.EmissionModule emission = psp.particleSystem.emission;
            emission.rateOverTime = psp.initialEmission;
        }
    }

    void OnNoDisaster()
    {
        ResetVolumes();
        ResetParticles();
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
