using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public static EventManager Singleton;

    //References

    //Public

    //Private

    void Awake()
    {
        Singleton = this;
    }

    public void OnDayStart()
    {
        //int totalDays = StoreManager.Singleton.TotalDaysPlayed;
        int day = StoreManager.Singleton.Day;
        int year = StoreManager.Singleton.Year;
        StoreManager.Season season = StoreManager.Singleton.CurrentSeason;

        //Easter : spring
        if (year > 1 && season == StoreManager.Season.Spring && (day == 8))
        {
            UIManager.Singleton.ShowMessage("Easter Time", "People are celebrating Easter!\nEggs and chocolate eggs are more popular!");
        }

        if (year > 1 && season == StoreManager.Season.Spring && (day == 8 || day == 9 || day == 10 || day == 11 || day == 12 || day == 13 || day == 14))
        {
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Eggs");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Chocolate Eggs");
        }

        if (year > 1 && season == StoreManager.Season.Spring && (day == 15))
        {
            UIManager.Singleton.ShowMessage("Easter Time Over", "Eggs are back to normal popularity");
        }

        // summer
        if (year > 1 && season == StoreManager.Season.Summer && (day == 8))
        {
            UIManager.Singleton.ShowMessage("It's a hot summer", "It's really hot outside!\nSoda, beer and ice cream are more popular!");
        }

        if (year > 1 && season == StoreManager.Season.Summer && (day == 8 || day == 9 || day == 10 || day == 11 || day == 12 || day == 13 || day == 14))
        {
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Soda");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Beer");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Ice Cream");
        }

        if (year > 1 && season == StoreManager.Season.Summer && (day == 15))
        {
            UIManager.Singleton.ShowMessage("The weather settled", "Soda, beer and ice cream are back to normal popularity");
        }

        // fall
        if (year > 0 && season == StoreManager.Season.Fall && (day == 8))
        {
            UIManager.Singleton.ShowMessage("Halloween time!", "It's almost halloween!\nPumpkins, apples and candy are more popular!");
        }

        if (year > 0 && season == StoreManager.Season.Fall && (day == 5 || day == 6 || day == 7 || day == 8))
        {
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Pumpkins");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Apples");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Sweets");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Cookies");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Toffees");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Happy Ranchers");
        }

        if (year > 0 && season == StoreManager.Season.Fall && (day == 15))
        {
            UIManager.Singleton.ShowMessage("Halloween ended", "Pumpkins, apples and candy are back to normal popularity");
        }

        // winter
        if (year > 0 && season == StoreManager.Season.Winter && (day == 8))
        {
            UIManager.Singleton.ShowMessage("Christmas time!", "It's the holiday season!\nPeople are festive and buying several kinds of gifts, foods & drinks!");
        }

        if (year > 0 && season == StoreManager.Season.Winter && (day == 5 || day == 6 || day == 7 || day == 8))
        {
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Toy Train");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Christmas Sweater");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Watch");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Cat Statue");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Stuffed Animal");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Turkey");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Eggnogg");
            EconomyManager.Singleton.ExtraPopularItemNames.Add("Champagne");
        }

        if (year > 0 && season == StoreManager.Season.Winter && (day == 15))
        {
            UIManager.Singleton.ShowMessage("Christmas ended", "Festive items are back to normal popularity");
        }

    }

    
}
