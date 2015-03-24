using UnityEngine;

/// <summary>
/// Used by simple UI elements to hold a reference to one of the possible stock items
/// </summary>
public class StockItemReferenceHolder : MonoBehaviour
{
    //References

    //Public
    public StockItem StockItemReference;

    //Private

    private void Awake()
    {
        StockItemReference = null;
    }
}