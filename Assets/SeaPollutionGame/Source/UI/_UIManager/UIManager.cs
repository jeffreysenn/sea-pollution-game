using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /*
     * TODO: add a new class for singleton
     */

    [Header("Managers and Controllers")]
    [SerializeField]
    private SingletonLevelManager _levelController = null;
    public SingletonLevelManager levelController { get { return _levelController; } }

    [SerializeField]
    private AttribLoader _attribLoader = null;
    public AttribLoader attribLoader { get { return _attribLoader; } }

    [SerializeField]
    private BaseEmissionManager _baseEmissionManager = null;
    public BaseEmissionManager baseEmissionManager { get { return _baseEmissionManager; } }

    [SerializeField]
    private DisasterManager _disasterManager = null;
    public DisasterManager disasterManager { get { return _disasterManager; } }

    [SerializeField]
    private PollutantMaterialManager _pollutantMaterialManager = null;
    public PollutantMaterialManager pollutantMaterialManager { get { return _pollutantMaterialManager; } }

    [SerializeField]
    private PlayerController _playerController = null;
    public PlayerController playerController { get { return _playerController; } }

    [SerializeField]
    private WorldStateManager _worldStateManager = null;
    public WorldStateManager worldStateManager { get { return _worldStateManager; } }

    [SerializeField]
    private SpaceManager _spaceManager = null;
    public SpaceManager spaceManager { get { return _spaceManager; } }

    [SerializeField]
    private FlowManager _flowManager = null;
    public FlowManager flowManager { get { return _flowManager; } }

    [SerializeField]
    private Player _player1 = null;
    public Player player1 { get { return _player1; } }

    [SerializeField]
    private Player _player2 = null;
    public Player player2 { get { return _player2; } }

    [SerializeField]
    private CameraManager _cameraManager = null;
    public CameraManager cameraManager { get { return _cameraManager; } }

    [Header("Introduction sequence")]
    [SerializeField]
    private bool startWithHowToPlay = true;
    [SerializeField]
    private HowToPlayMenu playMenu = null;
    [SerializeField]
    private FlagMenu flagMenu = null;

    // singleton
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    [SerializeField]
    private bool recordingDebug = false;
    [SerializeField]
    private int disasterDebug = 0;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        flagMenu.HideDirect();
        playMenu.HideDirect();
        
        if(startWithHowToPlay && SingletonLevelManager.instance.currentNbScenes <= 1)
        {
            playMenu.OnContinue += PlayMenu_OnContinue;
            playMenu.Show();
        } else
        {
            flagMenu.OnStart += FlagMenu_OnStart;
            flagMenu.Show();
        }
    }
    
    private void Update()
    {
        if(recordingDebug)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Disaster[] disasters = disasterManager.GetDisasters();
                foreach(Disaster d in disasters)
                {
                    d.chancePerTurn = 0;
                }
                Debug.Log("DEBUG UIManager: all disasters at 0");
            }

            if(Input.GetKeyDown(KeyCode.T))
            {
                Disaster[] disasters = disasterManager.GetDisasters();
                disasters[disasterDebug].chancePerTurn = 1;

                Debug.Log("DEBUG UIManager: " + disasters[disasterDebug].title + " at 1");
            }
        }
    }

    private void PlayMenu_OnContinue()
    {
        playMenu.OnContinue -= PlayMenu_OnContinue;
        playMenu.Hide();

        flagMenu.OnStart += FlagMenu_OnStart;
        flagMenu.Show();
    }

    private void FlagMenu_OnStart(CountryType obj)
    {
        flagMenu.OnStart -= FlagMenu_OnStart;
        flagMenu.Hide();
    }

    public void ChangeVolume(float value)
    {
        if (value < 0f && value > 1f) return;

        AudioListener.volume = value;
    }
    public float GetVolume() { return AudioListener.volume; }
}
