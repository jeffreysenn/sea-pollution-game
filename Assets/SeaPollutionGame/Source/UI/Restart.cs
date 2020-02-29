using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Restart : MonoBehaviour
{
    LevelController levelController = null;

    void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(()=> {
            levelController = FindObjectOfType<LevelController>();
            levelController.LoadRandomLevel();
        });
    }
}
