using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    //References

    //Public
    public ItemStockContainer LinkedItemStockContainer;


    //Private

    public void OpenStockItemWindow()
    {
        UIManager.Singleton.OpenStockSlotWindow(LinkedItemStockContainer);
    }

    public void OpenInfoWindow()
    {
        UIManager.Singleton.ShowObjectInfoScreen(LinkedItemStockContainer.transform);
    }
}