using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Purchasable
{
    public Vector3 offset;
    public Vector3 sclae;
    public int objCountPerRow;
    public float distanceX;
    public float distanceY;
    public GameObject polluter;
    public List<PolluterAttrib> polluterAttribs;
}

public class PurchaseMenu : MonoBehaviour
{
    [SerializeField] Text descriptionText = null;
    public List<Purchasable> purchasables = new List<Purchasable> { };

    Polluter InstantiatePolluter(GameObject obj, Vector3 pos, Vector3 scale)
    {
        var clone = Instantiate(obj, transform);
        clone.transform.localPosition = pos;
        clone.transform.localScale = scale;
        var drawDiscription = clone.GetComponent<DrawPolluterDescription>();
        drawDiscription.descriptionText = descriptionText;
        return clone.GetComponent<Polluter>();
    }

    void Start()
    {
        foreach (var pur in purchasables)
        {
            for(int i = 0; i != pur.polluterAttribs.Count; ++i)
            {
                int y = i / pur.objCountPerRow;
                int x = i - y * pur.objCountPerRow;
                var pos = pur.offset + new Vector3(pur.distanceX * x, 0, -pur.distanceY * y);
                var polluter = InstantiatePolluter(pur.polluter, pos, pur.sclae);
                polluter.SetAttrib(pur.polluterAttribs[i]);
                var textMesh = polluter.GetIdTextMesh();
                polluter.polluterId = (i + 1);
                textMesh.text = (i + 1).ToString();
            }
        }
    }
}
