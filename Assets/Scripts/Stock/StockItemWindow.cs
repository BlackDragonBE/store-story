using System;
using UnityEngine;
using UnityEngine.UI;

public class StockItemWindow : MonoBehaviour
{
    //References

    //Public
    public StockItemWindowType WindowType;

    [Header("References")]
    public Text TxtCategory;

    public ItemStockContainer ActiveContainer;

    [Header("Prefabs")]
    public GameObject StockItemEntryPrefab;

    //Private
    private StockItem.StockCategory _stockCategory = StockItem.StockCategory.FruitAndVeg;

    private int _stockIndex;
    private Transform _grid;

    public enum StockItemWindowType
    {
        Inventory, Order
    }

    private void Awake()
    {
        _stockIndex = (int)_stockCategory;
        _grid = transform.FindChild("Grid");
        TxtCategory.text = _stockCategory.ToString();

        //switch (WindowType)
        //{
        //    case StockItemWindowType.Inventory:
        //        LoadInventoryItems();
        //        break;
        //    case StockItemWindowType.Order:
        //        LoadOrderItems();
        //        break;
        //}
    }

    public void OnEnable()
    {
        switch (WindowType)
        {
            case StockItemWindowType.Inventory:
                LoadInventoryItems();
                break;

            case StockItemWindowType.Order:
                TutorialManager.Singleton.OnBuyGoodsWindowOpened();
                LoadOrderItems();
                break;
        }

    }

    public void OnDisable()
    {
        switch (WindowType)
        {
            case StockItemWindowType.Inventory:
        TutorialManager.Singleton.OnPlaceGoodsWindowClosed();
                break;

            case StockItemWindowType.Order:
        TutorialManager.Singleton.OnBuyGoodsWindowClosed();
                break;
        }

    }

    public void SetContainer(GameObject containerObject)
    {
        ActiveContainer = containerObject.GetComponent<ItemStockContainer>();
    }

    private void LoadInventoryItems()
    {
        //Clear all grid children
        ClearAllGridChildren();

        //Load items for current category
        FillInventoryItemList();
    }

    private void LoadOrderItems()
    {
        //Clear all grid children
        ClearAllGridChildren();

        //Load items for current category
        FillOrderItemList();
    }

    private void FillOrderItemList()
    {
        foreach (var stockItem in StockItemManager.Singleton.PossibleItems)
        {
            if (stockItem.Category == _stockCategory)
            {
                if (stockItem.UnlockLevel == 2 && !StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.StockIncrease1))
                {
                    continue;
                }

                if (stockItem.UnlockLevel == 3 && !StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.StockIncrease2))
                {
                    continue;
                }

                GameObject newStockEntry = Instantiate(StockItemEntryPrefab) as GameObject;
                newStockEntry.transform.SetParent(_grid, true);
                newStockEntry.transform.localScale = Vector3.one;
                newStockEntry.transform.FindChild("txtText").GetComponent<Text>().text = stockItem.Name + "\n" +
                                                                                         stockItem.MaxAmount + " pack" + "\n" + stockItem.Price.ToString("C") + " pp" + "\n" + (stockItem.Price * stockItem.MaxAmount).ToString("C");
                newStockEntry.transform.FindChild("Image").GetComponent<Image>().sprite =
                    StockItemManager.Singleton.GetInventorySprite(stockItem.SpriteName);

                newStockEntry.GetComponent<StockItemReferenceHolder>().StockItemReference = stockItem;

                //Sets the OnClick event for the button
                Button b = newStockEntry.GetComponent<Button>();
                b.GetComponent<Button>().onClick.AddListener(() => OnClickedStockItemForOrder(b));
            }
        }
    }

    private void FillInventoryItemList()
    {

        if (ActiveContainer.RestrictItemCategory)
        {
            _stockCategory = ActiveContainer.RestrictedCategories[0];
            TxtCategory.text = _stockCategory.ToString();
        }

        foreach (var stockItem in StoreManager.Singleton.Inventory)
        {
            if (stockItem.Category == _stockCategory)
            {
                GameObject newStockEntry = Instantiate(StockItemEntryPrefab) as GameObject;
                newStockEntry.transform.SetParent(_grid, true);
                newStockEntry.transform.localScale = Vector3.one;
                newStockEntry.transform.FindChild("txtText").GetComponent<Text>().text = stockItem.Name + "\n" +
                                                                                         stockItem.Amount + "/" + stockItem.MaxAmount + " pack" + "\n" + stockItem.Price.ToString("C") + " pp";
                newStockEntry.transform.FindChild("Image").GetComponent<Image>().sprite =
                    StockItemManager.Singleton.GetInventorySprite(stockItem.SpriteName);

                newStockEntry.GetComponent<StockItemReferenceHolder>().StockItemReference = stockItem;

                //Sets the OnClick event for the button
                Button b = newStockEntry.GetComponent<Button>();
                b.GetComponent<Button>().onClick.AddListener(() => OnClickedStockItemForSlotting(b));
            }
        }
    }

    private void OnClickedStockItemForOrder(Button button)
    {
        StockItem item = button.GetComponent<StockItemReferenceHolder>().StockItemReference;

        if (StoreManager.Singleton.Money >= item.Price * item.Amount)
        {
            //Player can buy it
            //UIManager.Singleton.ShowMessage("Ordered " + item.Name + "!", "One pack of " + item.Name + " has been delivered!\n(No delivery time yet!)");
            SoundManager.Singleton.PlayCashRegisterSound();

            //Pay
            StoreManager.Singleton.Money -= item.Price * item.Amount;

            //Add item to inventory
            //TODO: balance this, was 1.3f, maybe too low
            StoreManager.Singleton.Inventory.Add(StockItemManager.Singleton.GetStockItemCopy(item.Name, 1.7f));

            UIManager.Singleton.ShowFloatingText("Purchased a pack of " + item.Name , Color.green, Vector3.zero + new Vector3(0,2,0));

        }
        else
        {
            //Play error sound
            UIManager.Singleton.ShowMessage("Not enough money!", "You don't have enough money to order that pack!");
        }
    }

    private void OnClickedStockItemForSlotting(Button button)
    {
        StockItem item = button.GetComponent<StockItemReferenceHolder>().StockItemReference;
        //UIManager.Singleton.ShowMessage("Slotted " + item.Name + "!", "One pack of " + item.Name + " has been slotted!");

        if (ActiveContainer != null)
        {
            if (ActiveContainer.InventoryItem != null)
            {
                //Get item back first
                StoreManager.Singleton.Inventory.Add(ActiveContainer.InventoryItem);
            }

            ActiveContainer.SetItem(item);
            //Remove item to inventory
            StoreManager.Singleton.Inventory.Remove(item);
        }

        gameObject.SetActive(false);
        SoundManager.Singleton.PlayItemDropSound();
        //LoadInventoryItems();
    }

    public void NextCategory()
    {
        _stockIndex++;

        _stockIndex = _stockIndex % Enum.GetNames(typeof(StockItem.StockCategory)).Length; //Makes sure index can't be out of bounds
        _stockCategory = (StockItem.StockCategory)_stockIndex;

        TxtCategory.text = _stockCategory.ToString();

        switch (WindowType)
        {
            case StockItemWindowType.Inventory:
                LoadInventoryItems();
                break;

            case StockItemWindowType.Order:
                LoadOrderItems();
                break;
        }
    }

    public void PreviousCategory()
    {
        _stockIndex--;

        if (_stockIndex < 0)
        {
            _stockIndex = Enum.GetNames(typeof(StockItem.StockCategory)).Length - 1;
        }

        _stockCategory = (StockItem.StockCategory)_stockIndex;

        TxtCategory.text = _stockCategory.ToString();

        switch (WindowType)
        {
            case StockItemWindowType.Inventory:
                LoadInventoryItems();
                break;

            case StockItemWindowType.Order:
                LoadOrderItems();
                break;
        }
    }

    public void SetCategory(int categoryIndex)
    {

        if (categoryIndex == _stockIndex)
        {
            return;
        }

        SoundManager.Singleton.PlayButtonClickSound();

        _stockIndex = categoryIndex;
        _stockCategory = (StockItem.StockCategory) categoryIndex;

        TxtCategory.text = _stockCategory.ToString();

        switch (WindowType)
        {
            case StockItemWindowType.Inventory:
                LoadInventoryItems();
                break;

            case StockItemWindowType.Order:
                LoadOrderItems();
                break;
        }
    }

    private void ClearAllGridChildren()
    {
        _grid.DestroyAllChildren();
    }

}