public class DrawPolluterDescription : DrawDescription
{
    void Start()
    {
        var polluter = GetComponent<Polluter>();
        var attrib = polluter.GetAttrib();
        description = attrib.GetDescription();
    }
}
