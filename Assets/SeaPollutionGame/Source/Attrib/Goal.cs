[System.Serializable]
public class Goal
{
    public string title;
    public string resourceName;
    public float value;
    public float reward;
    public string description;
    public string iconName;

    public bool HasMetGoal(float val) { return val >= value; }
    public float GetProgress(float val) { return (val / value) > 1 ? 1 : val / value; }
}