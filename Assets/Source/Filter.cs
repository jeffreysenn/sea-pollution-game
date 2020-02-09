using UnityEngine;

public class Filter : Polluter {

    [SerializeField] FilterAttrib attrib = null;

    new void Start()
    {
        polluterAttrib = attrib;
        base.Start();
    }
}