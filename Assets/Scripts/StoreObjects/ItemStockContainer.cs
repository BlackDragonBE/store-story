using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to shelves, fridges, etc.
/// Holds an inventory item to sell
/// </summary>
public class ItemStockContainer : MonoBehaviour
{
    //References

    //Public
    public StockItem InventoryItem;
    public bool RestrictItemCategory;
    public List<StockItem.StockCategory> RestrictedCategories = new List<StockItem.StockCategory>();

    public Image ItemSprite;

    //Private

    void Awake()
    {
        InventoryItem = null;
    }

    public void SetItem(StockItem item)
    {
        if (item == null)
        {
            return;
        }

        InventoryItem = item;

        ItemSprite.sprite = StockItemManager.Singleton.GetInventorySprite(item.SpriteName);
        ItemSprite.enabled = true;
        UpdateRadial();
    }

    public void TakeItemsOutOfPack(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (InventoryItem != null)
            {
                InventoryItem.Amount--;
                UpdateRadial();

                if (InventoryItem.Amount == 0)
                {
                    InventoryItem = null;
                    ItemSprite.sprite = null;
                    ItemSprite.transform.parent.FindChild("Radial").GetComponent<Image>().fillAmount = 1f;
                    ItemSprite.enabled = false;
                }
            }
        }
    }

    private void UpdateRadial()
    {
        ItemSprite.transform.parent.FindChild("Radial").GetComponent<Image>().fillAmount = InventoryItem.Amount /
                                                                                           (float)InventoryItem.MaxAmount;
    }
}