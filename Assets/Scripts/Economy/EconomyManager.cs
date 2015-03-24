using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Singleton;

    //References

    //Public
    public StockItem.StockCategory TopCategory;
    public StockItem.StockCategory SecondCategory;
    public StockItem.StockCategory ThirdCategory;

    public int DaysSinceLastChange = 10;
    public Weather CurrentWeather = Weather.Comfortable;

    public bool JustLoadedSavedGame;

    public List<string> HotPopularItemNames = new List<string>();
    public List<string> ColdPopularItemNames = new List<string>();

    public List<string> ExtraPopularItemNames = new List<string>();

    public List<string> StockItemNamePool = new List<string>(); //Specific items get chosen randomly from this weighted list, weigth depends on item popularity and weather

    //Private
    void Awake()
    {
        Singleton = this;
    }

    public enum Weather
    {
        Comfortable, Cold, Hot
    }

    void Start()
    {
        //TopCategory = StockItem.StockCategory.Fruit;
        // SecondCategory = StockItem.StockCategory.Vegetables;
        // ThirdCategory = StockItem.StockCategory.BakedGoods;
    }

    public void OnNewDayStart()
    {
        DaysSinceLastChange++;

        //Don't change favorites & weather when loading from file
        if (!JustLoadedSavedGame)
        {
            if (DaysSinceLastChange >= Random.Range(2, 4))
            {
                SetNewFavorites();
                DaysSinceLastChange = 0;
            }

            ChangeWeather();
        }

        JustLoadedSavedGame = false;

        FillStockPool();
    }

    private void FillStockPool()
    {
        StockItemNamePool.Clear();

        foreach (var item in StockItemManager.Singleton.PossibleItems)
        {
            //Don't allow all item levels at the start!
            //Day ... - 53: Rank 1
            //Day 54 - 94: Rank 2
            //Day 94 - ...: Rank 3
            if (StoreManager.Singleton.TotalDaysPlayed < 53)
            {
                if (item.UnlockLevel == 2 || item.UnlockLevel == 3)
                {
                    continue;
                }
            }
            else if (StoreManager.Singleton.TotalDaysPlayed < 94)
            {
                if (item.UnlockLevel == 3)
                {
                    continue;
                }
            }

            //POPULAR CATEGORY
            if (item.Category == TopCategory)
            {
                for (int i = 0; i < 8; i++)
                {
                    StockItemNamePool.Add(item.Name);
                }
            }
            else if (item.Category == SecondCategory)
            {
                for (int i = 0; i < 4; i++)
                {
                    StockItemNamePool.Add(item.Name);
                }
            }
            else if (item.Category == ThirdCategory)
            {
                for (int i = 0; i < 2; i++)
                {
                    StockItemNamePool.Add(item.Name);
                }
            }
            else
            {
                StockItemNamePool.Add(item.Name);
            }

            //WEATHER
            if (CurrentWeather == Weather.Cold)
            {
                if (ColdPopularItemNames.Contains(item.Name))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        StockItemNamePool.Add(item.Name);
                    }
                }
            }
            else if (CurrentWeather == Weather.Hot)
            {
                if (HotPopularItemNames.Contains(item.Name))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        StockItemNamePool.Add(item.Name);
                    }
                }
            }

            //EXTRA POPULAR
            if (ExtraPopularItemNames.Contains(item.Name))
            {
                for (int i = 0; i < 500; i++)
                {
                    StockItemNamePool.Add(item.Name);
                }
            }
        }


        
        ExtraPopularItemNames.Clear();

    }

    private void ChangeWeather()
    {
        //Random weather
        int index = Random.Range(0, Enum.GetValues(typeof(Weather)).Length);
        CurrentWeather = (Weather)Enum.GetValues(typeof(Weather)).GetValue(index);

        UIManager.Singleton.OnWeatherUpdated();
    }

    private void SetNewFavorites()
    {
        if (StoreManager.Singleton.Day == 1)
        {
            TopCategory = StockItem.StockCategory.FruitAndVeg;
            SecondCategory = StockItem.StockCategory.Dairy;
            ThirdCategory = StockItem.StockCategory.BakedGoods;

            return;
        }

        List<StockItem.StockCategory> stockCategories = new List<StockItem.StockCategory>();

        do
        {
            //Get random index for a category
            int random = Random.Range(0, Enum.GetNames(typeof(StockItem.StockCategory)).Length);
            StockItem.StockCategory category =
                (StockItem.StockCategory)
                    Enum.Parse(typeof(StockItem.StockCategory), Enum.GetNames(typeof(StockItem.StockCategory))[random]);
            //Check if already in list
            if (!stockCategories.Contains(category))
            {
                stockCategories.Add(category);
            }

            //Add to list
        } while (stockCategories.Count < 3);

        TopCategory = stockCategories[0];
        SecondCategory = stockCategories[1];
        ThirdCategory = stockCategories[2];
    }
}