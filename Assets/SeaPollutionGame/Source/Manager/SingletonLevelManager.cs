using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class SingletonLevelManager : MonoBehaviour
{
    List<int> sceneIndice = new List<int> { };

    private static SingletonLevelManager _instance;
    public static SingletonLevelManager instance { get { return _instance; } }
    
    public int currentNbScenes;

    public void LoadRandomLevel()
    {
        if (sceneIndice.Count == 0) { GenRandomSceneList(); }
        int sceneToLoad = sceneIndice[0];
        sceneIndice.RemoveAt(0);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadHomeLevel()
    {
        SceneManager.LoadScene(0);
    }

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            _instance.currentNbScenes++;
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
            DontDestroyOnLoad(this);
            
            if (SceneManager.GetActiveScene().buildIndex == 0) { LoadRandomLevel(); }
        }
    }

    private void GenRandomSceneList()
    {
        var random = new System.Random();
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        foreach (int i in Enumerable.Range(0, sceneCount).OrderBy(x => random.Next()))
        {
            if (i != 0) { sceneIndice.Add(i); }
        }
    }
}
