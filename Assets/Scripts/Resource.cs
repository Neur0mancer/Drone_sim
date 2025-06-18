using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsTaken { get; private set; } = false;
    public float collectTime = 2f;

    public bool TryReserve()
    {
        if (IsTaken) return false;
        IsTaken = true;
        return true;
    }
     public void Collect()
    {
        Destroy(gameObject);
    }
}
