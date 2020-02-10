using UnityEngine;

[CreateAssetMenu(fileName = "New Pollution", menuName = "Pollution")]
public class Pollution : ScriptableObject
{
    public enum Type
    {
        TYPE_1,
        TYPE_2,
        TYPE_3,
        TYPE_4,
        TYPE_5,
        TYPE_6,
        TYPE_7,
        TYPE_8,
        TYPE_9,
        TYPE_10,
    }

    public Type type = Type.TYPE_1;
    public string pollutionName = "default pollution";
}
