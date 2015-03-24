using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Singleton;

    //References

    //Public
    //public string StoreName = "Unnamed Store";
    [Header("Debug")]
    [Range(0.1f, 20f)]
    public float TimeScale = 1f;

    [Header("Store Variables")]
    public float Money = 1000;
    public float Profit = 0;
    public float BillsToPayAtEndOfDay = 0;
    public int TimesBillsWerePaid = 0;

    public int Year = 1;
    public int Day = 1;
    public Season CurrentSeason = Season.Spring;
    public int TotalDaysPlayed = 0;
    public float StorePopularity;

    public bool StoreIsOpen;


    public Transform DoorLeft;
    public Transform DoorRight;

    public static Color StoreColor;
    public static string StoreName = "Epic Store";
    public static bool AdsVersion = false;

    //Prefabs
    [Header("Reference")]
    public Material WallMaterial;
    public List<FloorTile> FloorTiles = new List<FloorTile>();

    [Header("Prefabs")]
    public GameObject CustomerPrefab;

    //Important transforms
    [Header("Important Transforms")]
    public Transform StoreEntrance;
    public Transform CustomerBuyTransform;
    public Transform StoreClerk;

    public List<StockItem> Inventory = new List<StockItem>();
    public List<StoreUpgrade> ActiveUpgrades = new List<StoreUpgrade>();

    public TimeSpan StoreTime = new TimeSpan();
    //Private

    public enum Season
    {
        Spring, Summer, Fall, Winter
    }

    public enum StoreUpgrade
    {
        CashierProductivity1, CashierProductivity2, CashierProductivity3, Advertising1, Advertising2, Advertising3, StockIncrease1, StockIncrease2, StockIncrease3, CustomerCard
    }


    void Awake()
    {
        Singleton = this;
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly).SetCapacity(512, 32);
        DOTween.defaultEaseType = Ease.Linear;
    }

    void Start()
    {
        //Debug.Log(WebTools.GetNistTime().ToString("u"));
        StartDay();
        UIManager.Singleton.UpdateTime();
    }

    public void OnPlayerWantsToQuit()
    {
        UIManager.Singleton.ShowYesNoWindow("Quit Game?", "Are you sure you want to quit the game?\n\nWarning: all unsaved progress will be lost!", QuitGame, null);
    }

    public void QuitGame()
    {
        Application.LoadLevel("Title");
    }

    private void StartDay()
    {
        //SaveGameManager.Singleton.SaveGame();
        GC.Collect();
        StoreTime = TimeSpan.FromHours(8);
        EventManager.Singleton.OnDayStart();
        EconomyManager.Singleton.OnNewDayStart();
        UIManager.Singleton.OnStartDay();

        //First day
        if (TotalDaysPlayed == 0)
        {
            UIManager.Singleton.ShowMessage("Welcome to Store Story!", "This is your first day! How exciting!\n" +
                "As the manager of this store, it's your task to buy goods\n" +
                                                                       "and choose what to sell and when.\n" +
                                                                       "\n" +
                                                                       "Start off by pressing the Buy Shelves button\n" +
                                                                       "to place some shelves to put your goods in.");
        }

        //if (TotalDaysPlayed == 7)
        //{
        //    UIManager.Singleton.ShowMessage("Upgrades!", "You might have noticed already, but there\n" +
        //                                                 "are several upgrades available to the Upgrades menu.\n" +
        //                                                 "\n" +
        //                                                 "You can access the menu by pressing the Upgrades button.\n" +
        //                                                 "\n" +
        //                                                 "Be sure to check them out when you've made some money!");
        //}

        if (Day == 1 && TotalDaysPlayed != 0) //First day of season
        {
            UIManager.Singleton.ShowMessage(CurrentSeason.ToString(), "It's " + CurrentSeason + "!");
        }

        if (Day == 10 || Day == 20)
        {
            if (TotalDaysPlayed == 9)
            {
                UIManager.Singleton.ShowMessage("Bills", "Hi there!\nEvery 10 days you have to pay the bills at the end of the day.\nThis time I'll let you slide though.\n\nThe next payment is on the 20th of this season, it should be around $250,\nmake sure you're prepared!");
            }
            else
            {
                BillsToPayAtEndOfDay = 250f + TimesBillsWerePaid * 250f - TimesBillsWerePaid * 50;
                UIManager.Singleton.ShowMessage("Bills", "Total costs: " + BillsToPayAtEndOfDay.ToString("C") + "\n\nThis amount will be taken from your money at the end of the day.");
            }
        }
        UIManager.Singleton.UpdateTime();
    }

    public void OpenStore()
    {
        if (StoreObjectManager.Singleton.StoreObjects.Count == 0)
        {
            UIManager.Singleton.ShowMessage("Wait!", "You haven't placed any objects yet to sell!\nPlace at least one set of shelves and one pack of goods inside it.");
            return;
        }

        DoorLeft.DOLocalMoveZ(DoorLeft.transform.localPosition.z - 1, 2f);
        DoorRight.DOLocalMoveZ(DoorRight.transform.localPosition.z + 1, 2f);
        StoreIsOpen = true;
        UIManager.Singleton.OnOpenedStore();
        StoreTime = TimeSpan.FromHours(9);
        if (TotalDaysPlayed == 0)
        {
            UIManager.Singleton.ShowMessage("Store opened!", "The store is now open for business!\nCustomers will be coming in soon.\nGood luck!");
        }
        CustomerManager.Singleton.StartSpawningCustomers();
    }

    void Update()
    {
        Time.timeScale = TimeScale;

        if (StoreIsOpen)
        {
            StoreTime = StoreTime.Add(TimeSpan.FromMinutes(Time.deltaTime * 10));
            UIManager.Singleton.UpdateTime();

            if (StoreTime.Hours >= 18 && StoreTime.Minutes >= 0)
            {
                StoreIsOpen = false;
                CustomerManager.Singleton.StopSpawningCustomers();

                StartCoroutine(WaitForLastCustomerToLeaveAndCloseStore());
                //Close store
                //CloseStore();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !DragAndPlaceObject.Singleton.PlacingObject)
        {
            OnPlayerWantsToQuit();
        }

        //CHEATS
        if (Input.GetKeyDown(KeyCode.N))
        {
            Money += 1000;
        }
    }


    private IEnumerator WaitForLastCustomerToLeaveAndCloseStore()
    {
        while (CustomerManager.Singleton.Customers.Count > 0)
        {
            //this is to make sure the store not closing bug doesn't happen
            if (CustomerManager.Singleton.Customers[0] == null)
            {
                CustomerManager.Singleton.Customers.Clear();
            }

            yield return new WaitForSeconds(.3f);
        }

        //Customer left
        CloseStore();
    }

    private void CloseStore()
    {
        DoorLeft.DOLocalMoveZ(DoorLeft.transform.localPosition.z + 1, 2f);
        DoorRight.DOLocalMoveZ(DoorRight.transform.localPosition.z - 1, 2f);

        StoreIsOpen = false;
        UIManager.Singleton.OnClosedStore();
        UIManager.Singleton.ShowEndOfDayWindow();

        TutorialManager.Singleton.OnClosedStore();

        StorePopularity += CustomerManager.Singleton.HappyCustomers / 50f;
        StorePopularity -= CustomerManager.Singleton.SadCustomers / 50f;

        if (CustomerManager.Singleton.SadCustomers == 0)
        {
            //Give a little popularity boost
            StorePopularity += .25f;
        }

        StorePopularity = Mathf.Clamp(StorePopularity, 0f, 100f);

        Profit = 0;
        CustomerManager.Singleton.HappyCustomers = 0;
        CustomerManager.Singleton.NeutralCustomers = 0;
        CustomerManager.Singleton.SadCustomers = 0;

        if (BillsToPayAtEndOfDay > 0)
        {
            Money -= BillsToPayAtEndOfDay;
            TimesBillsWerePaid++;
            float nextBills = 250f + TimesBillsWerePaid * 250f - TimesBillsWerePaid * 50;
            UIManager.Singleton.ShowMessage("Payday", BillsToPayAtEndOfDay.ToString("C") + " has been deducted from your money to pay the bills.\nThe next bills will be " + nextBills.ToString("C") + ", dont't forget!");
            BillsToPayAtEndOfDay = 0;

            if (Money <= 0f)
            {
                GameOver();
                UIManager.Singleton.WindowEndOfDay.SetActive(false);
            }
        }
    }

    public float GetNextBillAmount()
    {
        return 250f + TimesBillsWerePaid * 250f - TimesBillsWerePaid * 50;
    }

    private void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        MusicManager.Singleton.FadeToSadMusic();
        UIManager.Singleton.HideInterface();

        yield return new WaitForSeconds(.5f);
        UIManager.Singleton.ShowMessage("Game Over", "You weren't able to pay the bills.\n" +
                                                     "I'm sorry but this is the end of " + StoreName + ".");

        yield return new WaitForSeconds(5f);

        UIManager.Singleton.ShowGameOverScreen();
    }

    public void GoToNextDay()
    {
        StartCoroutine(ToNextDayTransition());
    }

    private IEnumerator ToNextDayTransition()
    {
        AddDay();
        UIManager.Singleton.OnEndDay();

        //SaveGameManager.Singleton.SaveGame();

        yield return new WaitForSeconds(1f);

        StartDay();
    }

    private void AddDay()
    {
        TotalDaysPlayed++;
        Day++;

        if (Day == 21)
        {
            Day = 1;

            switch (CurrentSeason)
            {
                case Season.Spring:
                    CurrentSeason = Season.Summer;
                    break;

                case Season.Summer:
                    CurrentSeason = Season.Fall;
                    break;

                case Season.Fall:
                    CurrentSeason = Season.Winter;
                    break;

                case Season.Winter:
                    Year++;
                    CurrentSeason = Season.Spring;
                    break;
            }
        }
    }

    /// <summary>
    /// Searches all containers and returns the ones containing an item of the given category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public List<GameObject> GetContainersWithCategory(StockItem.StockCategory category)
    {
        //Find all store objects with a certain category of item slotted in them
        List<GameObject> outputList = new List<GameObject>();

        foreach (GameObject storeObject in StoreObjectManager.Singleton.StoreObjects)
        {
            if (storeObject.GetComponent<ItemStockContainer>().InventoryItem != null && storeObject.GetComponent<ItemStockContainer>().InventoryItem.Category == category)
            {
                outputList.Add(storeObject);
            }
        }

        return outputList;
    }

    /// <summary>
    /// Searches all containers and returns the ones containing an item of the given name
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public List<GameObject> GetContainersWithSpecificItem(string itemName)
    {
        List<GameObject> outputList = new List<GameObject>();

        foreach (GameObject storeObject in StoreObjectManager.Singleton.StoreObjects)
        {
            if (storeObject.GetComponent<ItemStockContainer>().InventoryItem != null && storeObject.GetComponent<ItemStockContainer>().InventoryItem.Name == itemName)
            {
                outputList.Add(storeObject);
            }
        }

        return outputList;
    }

    public void AddUpgrade(StoreUpgrade storeUpgrade)
    {
        switch (storeUpgrade)
        {
            case StoreUpgrade.CashierProductivity1:
                UIManager.Singleton.ShowMessage("Upgrade!", "You've upgraded your cashier!\nHe will handle customers faster than before!");
                break;
            case StoreUpgrade.CashierProductivity2:
                UIManager.Singleton.ShowMessage("Upgrade!", "You've upgraded your cashier!\nHe will handle customers faster than before!");
                break;
            case StoreUpgrade.CashierProductivity3:
                UIManager.Singleton.ShowMessage("Upgrade!", "You've upgraded your cashier!\nHe will handle customers faster than before!");
                break;
            case StoreUpgrade.Advertising1:
                UIManager.Singleton.ShowMessage("Upgrade!", "By advertising, you have gained some popularity!");
                StorePopularity += 2f;
                break;
            case StoreUpgrade.Advertising2:
                UIManager.Singleton.ShowMessage("Upgrade!", "By advertising, you have gained more popularity!");
                StorePopularity += 4f;
                break;
            case StoreUpgrade.Advertising3:
                UIManager.Singleton.ShowMessage("Upgrade!", "By advertising, you have gained even more popularity!");
                StorePopularity += 6f;
                break;
            case StoreUpgrade.StockIncrease1:
                UIManager.Singleton.ShowMessage("Upgrade!", "You've unlocked new goods to buy!");
                break;
            case StoreUpgrade.StockIncrease2:
                UIManager.Singleton.ShowMessage("Upgrade!", "You've unlocked even more new goods to buy!");
                break;
            case StoreUpgrade.StockIncrease3:
                break;
            case StoreUpgrade.CustomerCard:
                UIManager.Singleton.ShowMessage("Upgrade!", "Customer interests are no mystery to you anymore!\nTake a look at the Store Info window to see what's popular!");
                break;
        }

        ActiveUpgrades.Add(storeUpgrade);
    }

    public void ShowAd()
    {
        StartCoroutine(AdRoutine());

    }

    private IEnumerator AdRoutine()
    {
        UIManager.Singleton.BtnShowAd.SetActive(false);
        UIManager.Singleton.TxtLoadingAd.gameObject.SetActive(true);

        if (Advertisement.isReady())
        {
            Advertisement.Show(null, new ShowOptions() { pause = true, resultCallback = ResultCallback });
            UIManager.Singleton.TxtLoadingAd.gameObject.SetActive(false);
        }
        else
        {
            Advertisement.Initialize("131625348");

            yield return new WaitForSeconds(1f);

            if (Advertisement.isReady())
            {
                Advertisement.Show(null, new ShowOptions() { pause = true, resultCallback = ResultCallback });
                UIManager.Singleton.TxtLoadingAd.gameObject.SetActive(false);
                yield break;;
            }

            yield return new WaitForSeconds(2f);
            if (Advertisement.isReady())
            {
                Advertisement.Show(null, new ShowOptions() { pause = true, resultCallback = ResultCallback });
                UIManager.Singleton.TxtLoadingAd.gameObject.SetActive(false);
                yield break; ;
            }

            yield return new WaitForSeconds(1f);
            if (Advertisement.isReady())
            {
                Advertisement.Show(null, new ShowOptions() { pause = true, resultCallback = ResultCallback });
                UIManager.Singleton.TxtLoadingAd.gameObject.SetActive(false);
                yield break; ;
            }
            
            //UIManager.Singleton.ShowMessage("No ads available", "Ads couldn't be loaded, please make sure\n" +
            //                                                    "you are connected to the internet and you disabled\n" +
            //                                                    "any ad-blocking software.");

            UIManager.Singleton.ShowMessage("Ads unavailable", "Ads couldn't be loaded.\n" +
                                                               "Please show your support by buying the full game.\n" +
                                                               "Loading next day in 5 seconds...");
            UIManager.Singleton.TxtLoadingAd.gameObject.SetActive(false);
            yield return new WaitForSeconds(5f);
            GoToNextDay();
        }
    }

    private void ResultCallback(ShowResult showResult)
    {
        //Debug.Log(showResult.ToString());
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                GoToNextDay();
                break;
            case ShowResult.Finished:
                GoToNextDay();
                break;
        }
    }
}