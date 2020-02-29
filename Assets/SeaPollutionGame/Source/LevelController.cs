using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class LevelController : MonoBehaviour
{
    List<int> sceneIndice = new List<int> { };
    private static LevelController instance = null;

    public void LoadRandomLevel()
    {
        if (sceneIndice.Count == 0) { GenRandomSceneList(); }
        int sceneToLoad = sceneIndice[0];
        sceneIndice.RemoveAt(0);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadHomeLevel()
    {
        SceneManager.LoadScene(0);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //if(SceneManager.GetActiveScene().buildIndex == 0) { LoadRandomLevel(); }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
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
