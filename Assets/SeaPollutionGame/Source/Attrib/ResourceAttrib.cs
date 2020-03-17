[System.Serializable]
public class ResourceAttrib
{
    [System.Serializable]
    public class Product
    {
        public string resourceName = "";
        public float productPerTurn = 0;
    }

    public Product[] products = null;
}
