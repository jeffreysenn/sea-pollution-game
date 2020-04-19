using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DisasterEvent : UnityEvent<Disaster> { }

public class DisasterManager : MonoBehaviour
{
    [System.Serializable]
    class DisasterSkybox
    {
        public string disasterName = null;
        public Texture front = null;
        public Texture back = null;
        public Texture left = null;
        public Texture right = null;
        public Texture up = null;
        public Texture down = null;
    }

    Disaster[] disasters = null;
    WorldStateManager stateManager = null;
    DisasterEvent disasterEvent = new DisasterEvent { };
    UnityEvent noDisasterEvent = new UnityEvent { };
    CameraManager cameraManager = null;

    [Header("Skyboxes")]
    [SerializeField]
    private Material defaultTexture = null;
    [SerializeField]
    private Material skyboxTextureReference = null;
    [SerializeField]
    private DisasterSkybox defaultSkybox;
    [SerializeField]
    private List<DisasterSkybox> disasterSkyboxes = null;

    private Material skyboxTexture = null;

    public void SetDisasters(Disaster[] arr)
    {
        disasters = new Disaster[arr.Length];
        System.Array.Copy(arr, disasters, arr.Length);
    }

    public void AddDisasterEventListener(UnityAction<Disaster> action) { disasterEvent.AddListener(action); }
    public void AddNoDisasterEventListener(UnityAction action) { noDisasterEvent.AddListener(action); }

    void Start()
    {
        stateManager = FindObjectOfType<WorldStateManager>();
        cameraManager = FindObjectOfType<CameraManager>();

        skyboxTexture = new Material(skyboxTextureReference);
        RenderSettings.skybox = skyboxTexture;
        SetSkybox(defaultSkybox, false);

        stateManager.AddEndTurnEventListener(GenDisaster);
    }

    private void OnDestroy()
    {
        RenderSettings.skybox = defaultTexture;
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
                    OnDisaster(disaster);
                    return;
                }
            }
            OnNoDisaster();
        }
    }

    void OnDisaster(Disaster disaster)
    {
        disasterEvent.Invoke(disaster);

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
                health.AddHealth(-damage, disaster);
            }
        }

        cameraManager.SetState(CameraManager.State.CINEMA);

        //change skybox
        foreach(DisasterSkybox ds in disasterSkyboxes)
        {
            if(ds.disasterName == disaster.title)
            {
                SetSkybox(ds);
                break;
            }
        }
    }

    void OnNoDisaster()
    {
        noDisasterEvent.Invoke();

        SetSkybox(defaultSkybox);
    }

    void SetSkybox(DisasterSkybox ds, bool transition = true)
    {
        /*
        _FrontTex("Front (+Z)", 2D) = "white" {}
		_BackTex("Back (-Z)", 2D) = "white" {}
		_LeftTex("Left (+X)", 2D) = "white" {}
		_RightTex("Right (-X)", 2D) = "white" {}
		_UpTex("Up (+Y)", 2D) = "white" {}
		_DownTex("Down (-Y)", 2D) = "white" {}
		_FrontTex2("2 Front (+Z)", 2D) = "white" {}
		_BackTex2("2 Back (-Z)", 2D) = "white" {}
		_LeftTex2("2 Left (+X)", 2D) = "white" {}
		_RightTex2("2 Right (-X)", 2D) = "white" {}
		_UpTex2("2 Up (+Y)", 2D) = "white" {}
		_DownTex2("2 Down (-Y)", 2D) = "white" {}
        */

        // the end of the texture becomes the start of the shader
        RenderSettings.skybox.SetTexture("_FrontTex", RenderSettings.skybox.GetTexture("_FrontTex2"));
        RenderSettings.skybox.SetTexture("_BackTex", RenderSettings.skybox.GetTexture("_BackTex2"));
        RenderSettings.skybox.SetTexture("_LeftTex", RenderSettings.skybox.GetTexture("_LeftTex2"));
        RenderSettings.skybox.SetTexture("_RightTex", RenderSettings.skybox.GetTexture("_RightTex2"));
        RenderSettings.skybox.SetTexture("_UpTex", RenderSettings.skybox.GetTexture("_UpTex2"));
        RenderSettings.skybox.SetTexture("_DownTex", RenderSettings.skybox.GetTexture("_DownTex2"));
        
        // new texture at the end of the shader
        RenderSettings.skybox.SetTexture("_FrontTex2", ds.front);
        RenderSettings.skybox.SetTexture("_BackTex2", ds.back);
        RenderSettings.skybox.SetTexture("_LeftTex2", ds.left);
        RenderSettings.skybox.SetTexture("_RightTex2", ds.right);
        RenderSettings.skybox.SetTexture("_UpTex2", ds.up);
        RenderSettings.skybox.SetTexture("_DownTex2", ds.down);

        if(transition)
        {
            StopCoroutine(SkyboxFadeCoroutine());
            StartCoroutine(SkyboxFadeCoroutine());
        } else
        {
            RenderSettings.skybox.SetFloat("_Blend", 1f);
        }
    }

    IEnumerator SkyboxFadeCoroutine(bool fadeIn = true)
    {
        float i;

        if (fadeIn)
            i = 0f;
        else
            i = 1f;
        
        RenderSettings.skybox.SetFloat("_Blend", i);

        if(fadeIn)
        {
            while (i < 1f)
            {
                i += Time.deltaTime;
                RenderSettings.skybox.SetFloat("_Blend", i);
                yield return null;
            }
        } else
        {
            while (i > 0f)
            {
                i -= Time.deltaTime;
                RenderSettings.skybox.SetFloat("_Blend", i);
                yield return null;
            }
        }

        if(fadeIn)
            RenderSettings.skybox.SetFloat("_Blend", 1f);
        else
            RenderSettings.skybox.SetFloat("_Blend", 0f);
    }
}
