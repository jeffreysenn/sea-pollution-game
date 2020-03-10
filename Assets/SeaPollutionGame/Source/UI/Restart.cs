using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Restart : MonoBehaviour
{
    SingletonLevelManager levelManager = null;

    void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            levelManager = FindObjectOfType<SingletonLevelManager>();
            levelManager.LoadRandomLevel();
        });
    }
}
