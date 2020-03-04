using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    /*
     * TODO: add a new class for singleton
     */

    [SerializeField]
    private LevelController _levelController = null;
    public LevelController levelController { get { return _levelController; } }

    [SerializeField]
    private AttribLoader _attribLoader = null;
    public AttribLoader attribLoader { get { return _attribLoader; } }

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
    private Player _player1 = null;
    public Player player1 { get { return _player1; } }

    [SerializeField]
    private Player _player2 = null;
    public Player player2 { get { return _player2; } }

    // singleton
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }


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
}
