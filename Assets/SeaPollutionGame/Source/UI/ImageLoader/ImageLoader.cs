using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ImageLoader : MonoBehaviour
{
    [SerializeField]
    private Image targetImage = null;
    [SerializeField]
    private string imageExtension = ".png";
    [SerializeField]
    private string path = "Images/Components/";

    List<Sprite> sprites = new List<Sprite>();

    private void Awake()
    {
        // preload images?
        sprites = Resources.LoadAll(path, typeof(Sprite)).Cast<Sprite>().ToList();
    }

    public void LoadImage(string imageName)
    {
        Sprite image = sprites.Find(x => x.name.ToLower().Equals(imageName.ToLower()));

        if (image == null) { Debug.LogError("[ImageLoader] LoadImage: image doesn't exist: " + imageName); return; }

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
