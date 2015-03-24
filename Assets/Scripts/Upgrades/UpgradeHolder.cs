using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeHolder : MonoBehaviour
{
    //References

    //Public
    public StoreManager.StoreUpgrade Upgrade;
    public float Price = 10000;

    //Private
    private Text _txtName;

    void Start()
    {
        _txtName = transform.FindChild("txtName").GetComponent<Text>();

        for (int i = 0; i < 4; i++)
        {
            if (StoreManager.Singleton.ActiveUpgrades.Contains(Upgrade))
            {
                UpdateUpgrade();
            }
        }

        UpdateButton();
    }

    private void UpdateButton()
    {
        transform.Find("BtnBuy/txtBuy").GetComponent<Text>().text = "Buy\n" + Price.ToString("C");
    }

    public void AddUpgradeToStore()
    {
        if (StoreManager.Singleton.Money >= Price)
        {
            StoreManager.Singleton.Money -= Price;

            StoreManager.Singleton.ActiveUpgrades.Add(Upgrade);
            StoreManager.Singleton.AddUpgrade(Upgrade);
            SoundManager.Singleton.PlayCashRegisterSound();

            UpdateUpgrade();
            //gameObject.SetActive(false);
        }
        else
        {
            UIManager.Singleton.ShowMessage("Not enough money!","You lack the money to buy this upgrade");
        }

        UpdateButton();
    }

    private void UpdateUpgrade()
    {
        switch (Upgrade)
        {
            case StoreManager.StoreUpgrade.CashierProductivity1:
                Upgrade = StoreManager.StoreUpgrade.CashierProductivity2;
                _txtName.text = "Cashier Productivity Lessons 2";
                Price = 3200;
                break;
            case StoreManager.StoreUpgrade.CashierProductivity2:
                Upgrade = StoreManager.StoreUpgrade.CashierProductivity3;
                _txtName.text = "Cashier Productivity Lessons 3";
                Price = 6000;
                break;
            case StoreManager.StoreUpgrade.CashierProductivity3:
                gameObject.SetActive(false);
                break;
            case StoreManager.StoreUpgrade.Advertising1:
                Upgrade = StoreManager.StoreUpgrade.Advertising2;
                Price = 4500;
                _txtName.text = "Advertising 2";
                break;
            case StoreManager.StoreUpgrade.Advertising2:
                Upgrade = StoreManager.StoreUpgrade.Advertising3;
                _txtName.text = "Advertising 3";
                Price = 7500;
                break;
            case StoreManager.StoreUpgrade.Advertising3:
                gameObject.SetActive(false);
                break;
            case StoreManager.StoreUpgrade.StockIncrease1:
                Upgrade = StoreManager.StoreUpgrade.StockIncrease2;
                _txtName.text = "Stock Increase 2";
                Price = 3200;
                break;
            case StoreManager.StoreUpgrade.StockIncrease2:
                gameObject.SetActive(false);
                break;
            case StoreManager.StoreUpgrade.StockIncrease3:
                break;
            case StoreManager.StoreUpgrade.CustomerCard:
                gameObject.SetActive(false);
                break;
        }
    }
}
