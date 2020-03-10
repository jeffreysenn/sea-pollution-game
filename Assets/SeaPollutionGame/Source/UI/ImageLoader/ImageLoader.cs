using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageLoader : MonoBehaviour
{
    [SerializeField]
    private Image targetImage = null;
    [SerializeField]
    private string imageExtension = ".png";
    [SerializeField]
    private string path = "Images/";

    private void Awake()
    {
        // preload images?
    }

    public void LoadImage(string imageName)
    {
        string file = path + imageName;

        Sprite image = Resources.Load<Sprite>(imageName);

        if(image == null) { Debug.LogError("[ImageLoader] LoadImage: image doesn't exist: " + file); return; }

        SetImage(image);
    }

    public void SetImage(Sprite sprite)
    {
        targetImage.overrideSprite = sprite;
        targetImage.preserveAspect = true;
    }

    public void Clear()
    {
        targetImage.overrideSprite = null;
    }
}
