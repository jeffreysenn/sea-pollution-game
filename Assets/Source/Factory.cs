using UnityEngine;

public class Factory : Polluter
{
    [SerializeField] FactoryAttrib attrib = null;

    new void Start()
    {
        polluterAttrib = attrib;
        base.Start();
    }
}
