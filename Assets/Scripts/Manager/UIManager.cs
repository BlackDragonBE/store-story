using System;
using System.Text;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton;

    //References

    //Public
    [Header("DEBUG")]
    public bool DebugSkipPolish;

    public Sprite TreeSpring;
    public Sprite TreeSummer;
    public Sprite TreeFall;
    public Sprite TreeWinter;

    public Sprite WeatherCold;
    public Sprite WeatherComfortable;
    public Sprite WeatherHot;

    [Header("References")]
    public Transform Canvas;

    public Transform ControlsPanel;

    public Text TxtMoney;
    public Text TxTime;
    public Text TxtStorePopularity;
    public Text TxtBlackOverlayDay;
    public Text TxtLoadingAd;

    public Toggle ToggleNormalSpeed;
    public Toggle ToggleMusic;
    public Toggle ToggleSound;

    //quality
    public Toggle ToggleQualityFast;
    public Toggle ToggleQualityGood;
    public Toggle ToggleQualityBeautiful;
    public Toggle ToggleQualityFantastic;

    //Weather
    public Image ImgTemperature;

    public Image BlackOverlayImage;
    public Image ImgSeasonTree;

    public GameObject MessageBox;
    public YesNoDialogWindow YesNoWindow;
    public GameObject FloatingTextPrefab;

    public GameObject WindowEndOfDay;
    public GameObject WindowObjectInfo;

    public GameObject SaveGameIcon;

    public GameObject BtnBuyStorage;
    public GameObject BtnBuyGoods;
    public GameObject BtnOpenShop;
    public GameObject BtnNextDay;
    public GameObject BtnSaveGame;
    public GameObject BtnUpgrades;
    public GameObject BtnShowAd;

    //Save game
    public Transform SaveSlot1;
    public Transform SaveSlot2;
    public Transform SaveSlot3;

    public GameObject GameOverScreen;

    public GameObject WindowStockItemsInventory;
    public GameObject WindowStoreInfo;

    public RectTransform WeReOpenSign;

    public List<GameObject> ItemSlots = new List<GameObject>();

    public Light DirLight;

    public GameObject BottomPanel;
    public GameObject InfoButton;
    public GameObject OptionsButton;
    public GameObject SpeedPanel;

    [Header("Prefabs")]
    public GameObject ItemSlotPrefab;

    //Private
    private float _previousMoney;
    private float _previousPopularity;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //ShowFloatingText("Test", Color.green, new Vector3(2,2,2));
        //StartCoroutine(FadeToBlack());
        SetQualityToggle();

        if (DebugSkipPolish)
        {
            ShowFloatingText("DEBUG: Polish skip mode active", Color.white, Vector3.zero);
        }

        UpdateTime();
    }
    private void Update()
    {
        if (StoreManager.Singleton.Money != _previousMoney)
        {
            TxtMoney.text = StoreManager.Singleton.Money.ToString("C");
        }

        if (StoreManager.Singleton.StorePopularity != _previousPopularity)
        {
            TxtStorePopularity.text = "Popularity " + StoreManager.Singleton.StorePopularity.ToString("N") + " %";
        }

        _previousMoney = StoreManager.Singleton.Money;
        _previousPopularity = StoreManager.Singleton.StorePopularity;
    }

    public void UpdateTime()
    {
        TxTime.text = "Y" + StoreManager.Singleton.Year + " D" + StoreManager.Singleton.Day + "\n" +
                      StoreManager.Singleton.CurrentSeason + "\n" + StoreManager.Singleton.StoreTime.Hours.ToString("00") +
                      ":" +
                      StoreManager.Singleton.StoreTime.Minutes.ToString("00");
    }

    public void ShowMessage(string title, string text)
    {
        MessageBox.SetActive(true);
        DOTween.Kill(MessageBox);
        MessageBox.transform.localScale = Vector3.one;

        MessageBox.transform.Find("Title/txtTitle").GetComponent<Text>().text = title;
        MessageBox.transform.FindChild("txtText").GetComponent<Text>().text = text;

        //Automatically resize messagebox
        float prefTextWidth =
            LayoutUtility.GetPreferredWidth(MessageBox.transform.FindChild("txtText").GetComponent<RectTransform>());
        float prefTextHeight =
            LayoutUtility.GetPreferredHeight(MessageBox.transform.Find("txtText").GetComponent<RectTransform>());

        float prefTitleWidth = LayoutUtility.GetPreferredWidth(MessageBox.transform.FindChild("Title/txtTitle").GetComponent<RectTransform>());

        if (prefTextWidth < prefTitleWidth)
        {
            prefTextWidth = prefTitleWidth;
        }

        MessageBox.GetComponent<RectTransform>().sizeDelta = new Vector2(prefTextWidth + 100, prefTextHeight + 160);

        MessageBox.transform.DOScale(new Vector3(.5f, .5f, .5f), .2f).From();
    }

    public void OnStartDay()
    {
        ToggleNormalSpeed.isOn = true;
        WeReOpenSign.anchoredPosition = new Vector2(85, 100);

        BtnNextDay.SetActive(false);
        BtnOpenShop.SetActive(true);
        BtnBuyStorage.SetActive(true);
        BtnBuyGoods.SetActive(true);
        BtnSaveGame.SetActive(true);
        BtnUpgrades.SetActive(true);
        BtnShowAd.SetActive(false);

        switch (StoreManager.Singleton.CurrentSeason)
        {
            case StoreManager.Season.Spring:
                ImgSeasonTree.sprite = TreeSpring;
                break;
            case StoreManager.Season.Summer:
                ImgSeasonTree.sprite = TreeSummer;
                break;
            case StoreManager.Season.Fall:
                ImgSeasonTree.sprite = TreeFall;
                break;
            case StoreManager.Season.Winter:
                ImgSeasonTree.sprite = TreeWinter;
                break;
        }

        //ShowMessage("Today's popular item categories", string.Format("1.{0}\n2.{1}\n3.{2}", EconomyManager.Singleton.TopCategory, EconomyManager.Singleton.SecondCategory, EconomyManager.Singleton.ThirdCategory));

        if (DebugSkipPolish)
        {
            return;
        }

        BlackOverlayImage.color = new Color(0, 0, 0, 255f);
        BlackOverlayImage.gameObject.SetActive(true);
        //StartCoroutine(DayStartRoutine());

        StartCoroutine(DayStartRoutine());
    }

    //NOT USED:
    public void OnInfoButtonClicked()
    {
        WindowStoreInfo.SetActive(true);
        WindowStoreInfo.transform.FindChild("ImgHideFavorites").gameObject.SetActive(true);
        WindowStoreInfo.transform.Find("Grid/txtTotalDaysPlayed").GetComponent<Text>().text =
            StoreManager.Singleton.TotalDaysPlayed.ToString();
        WindowStoreInfo.transform.Find("Grid/txtNextBills").GetComponent<Text>().text =
    StoreManager.Singleton.GetNextBillAmount().ToString("C");

        if (StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.CustomerCard))
        {
            WindowStoreInfo.transform.FindChild("ImgHideFavorites").gameObject.SetActive(false);
            WindowStoreInfo.transform.Find("txtFavoriteCategories").GetComponent<Text>().text =
        "1. " + EconomyManager.Singleton.TopCategory + "\n" +
        "2. " + EconomyManager.Singleton.SecondCategory + "\n" +
        "3. " + EconomyManager.Singleton.ThirdCategory;
        }



        //StringBuilder sb = new StringBuilder(1024);
        //sb.Append("Total days played: ");
        //sb.Append(StoreManager.Singleton.TotalDaysPlayed);

        //if (StoreManager.Singleton.ActiveUpgrades.Contains(StoreManager.StoreUpgrade.CustomerCard))
        //{
        //    sb.Append("\n");
        //    sb.Append("1. ");
        //    sb.Append(EconomyManager.Singleton.TopCategory);
        //    sb.Append("\n");
        //    sb.Append("2. ");
        //    sb.Append(EconomyManager.Singleton.SecondCategory);
        //    sb.Append("\n");
        //    sb.Append("3. ");
        //    sb.Append(EconomyManager.Singleton.ThirdCategory);
        //}

        //sb.Append("\n");
        //sb.Append("Next bill: ");
        //sb.Append(StoreManager.Singleton.GetNextBillAmount().ToString("C"));

        //ShowMessage("Info", sb.ToString());
    }

    private IEnumerator DayStartRoutine()
    {
        yield return new WaitForSeconds(1f);
        TxtBlackOverlayDay.GetComponent<Text>().text = StoreManager.StoreName + "\n" + StoreManager.Singleton.CurrentSeason + "\n" + "Day " + StoreManager.Singleton.Day;
        //TxtBlackOverlayDay.GetComponent<Text>().color = new Color(0, 0, 0, 0);
        TxtBlackOverlayDay.gameObject.SetActive(true);
        //yield return new WaitForSeconds(.4f);

        TxtBlackOverlayDay.GetComponent<TextTypewriterEffect>().StartTypeWriter();
        //Go.to(TxtBlackOverlayDay.transform.GetComponent<Text>(), .8f,
        //   new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 1f)));
        //TxtBlackOverlayDay.transform.GetComponent<Text>().DOColor(new Color(1, 1, 1, 1), .8f);

        //SoundManager.Singleton.PlayNewDaySound();

        yield return new WaitForSeconds(2f);

        //Go.to(TxtBlackOverlayDay.transform.GetComponent<Text>(), .6f,
        //   new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f)));
        //TxtBlackOverlayDay.transform.GetComponent<Text>().DOColor(new Color(0, 0, 0, 0), .6f);

        yield return new WaitForSeconds(.7f);

        TxtBlackOverlayDay.gameObject.SetActive(false);
        StartCoroutine(FadeFromBlack());
    }

    public void OnOpenedStore()
    {
        //WeReOpenSign.anchoredPosition = new Vector2(85, 100);
        WeReOpenSign.DOAnchorPos(new Vector2(85, -40), .5f);

        //Hide buy buttons & shop open button
        BtnBuyStorage.SetActive(false);
        BtnBuyGoods.SetActive(false);
        BtnOpenShop.SetActive(false);
        BtnSaveGame.SetActive(false);
        BtnUpgrades.SetActive(false);
    }

    public void OnClosedStore()
    {
        WeReOpenSign.DOAnchorPos(new Vector2(85, 100), .5f);

        //Show buy buttons & shop open button
        BtnNextDay.SetActive(true);
        ToggleNormalSpeed.isOn = true;

        //If ads version and total days is multiple of 5, show ad button instead of next day button
        if (StoreManager.AdsVersion && StoreManager.Singleton.TotalDaysPlayed != 0 && StoreManager.Singleton.TotalDaysPlayed % 5 == 0)
        {
            ShowMessage("Ad Version","This is an ad-supported version of Store Story.\n" +
                                     "Every 5 days you will get this message.\n" +
                                     "\n" +
                                     "Make sure you've got an internet connection and press the\n" +
                                     "Show ad button to go to the next day.\n" +
                                     "\n" +
                                     "Please purchase the full version to disable ads.");
            BtnNextDay.SetActive(false);
            BtnShowAd.SetActive(true);
        }
    }

    public void OnEndDay()
    {
        if (DebugSkipPolish)
        {
            return;
        }

        StartCoroutine(FadeToBlack());
    }

    public void ShowYesNoWindow(string title, string question, Action onYes, Action onNo)
    {
        YesNoWindow.SetYesNoActions(onYes, onNo);
        YesNoWindow.Show(title, question);
        YesNoWindow.transform.DOScale(new Vector3(.5f, .5f, .5f), .2f).From();
    }

    public void ShowFloatingText(string text, Color color, Vector3 worldPosition)
    {
        GameObject floatText = Instantiate(FloatingTextPrefab) as GameObject;
        floatText.transform.SetParent(Canvas);
        floatText.transform.localScale = Vector3.one;
        floatText.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(worldPosition);
        //floatText.transform.positionTo(.5f, new Vector3(0, 50, 0), true);
        floatText.GetComponent<RectTransform>().DOMoveY(floatText.transform.position.y + 50, .5f, true);
        floatText.GetComponent<Text>().text = text;
        floatText.GetComponent<Text>().color = color;
        floatText.GetComponent<Outline>().effectColor = new Color(color.r - .5f, color.g - .5f, color.b - .5f);
        Destroy(floatText, 1.5f);
    }

    public void ShowEndOfDayWindow()
    {
        WindowEndOfDay.transform.Find("Grid/txtProfit").GetComponent<Text>().text = "$" + StoreManager.Singleton.Profit.ToString("#0.00");
        WindowEndOfDay.transform.Find("Grid/txtHappyCust").GetComponent<Text>().text = CustomerManager.Singleton.HappyCustomers.ToString();
        WindowEndOfDay.transform.Find("Grid/txtNeutralCust").GetComponent<Text>().text = CustomerManager.Singleton.NeutralCustomers.ToString();
        WindowEndOfDay.transform.Find("Grid/txtSadCust").GetComponent<Text>().text = CustomerManager.Singleton.SadCustomers.ToString();
        WindowEndOfDay.SetActive(true);
    }

    public void CreateItemSlotForContainer(ItemStockContainer container)
    {
        GameObject itemSlot = Instantiate(ItemSlotPrefab);
        itemSlot.transform.SetParent(Canvas);
        itemSlot.transform.localScale = Vector3.one;
        itemSlot.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(container.transform.position + new Vector3(0, 2, 0));
        itemSlot.GetComponent<ItemSlot>().LinkedItemStockContainer = container;
        itemSlot.transform.SetAsFirstSibling();

        container.GetComponent<ItemStockContainer>().ItemSprite =
            itemSlot.transform.FindChild("Image").GetComponent<Image>();

        ItemSlots.Add(itemSlot);
    }

    public void OpenStockSlotWindow(ItemStockContainer itemStockContainer)
    {
        WindowStockItemsInventory.GetComponent<StockItemWindow>().ActiveContainer = itemStockContainer;
        WindowStockItemsInventory.SetActive(true);
    }

    public IEnumerator FadeToBlack()
    {
        BlackOverlayImage.color = new Color(0, 0, 0, 0);
        BlackOverlayImage.gameObject.SetActive(true);

        //Go.to(BlackOverlayImage.transform.GetComponent<Image>(), 1f,
        //           new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 1f)));

        BlackOverlayImage.transform.GetComponent<Image>().DOFade(1f, 1f);
        yield return null;
    }

    public IEnumerator FadeFromBlack()
    {
        BlackOverlayImage.color = new Color(0, 0, 0, 1f);
        BlackOverlayImage.gameObject.SetActive(true);

        //Go.to(BlackOverlayImage.transform.GetComponent<Image>(), 1f,
        //           new GoTweenConfig().colorProp("color", new Color(0, 0, 0, 0f)));
        BlackOverlayImage.transform.GetComponent<Image>().DOFade(0f, 1f);

        yield return new WaitForSeconds(1.05f);
        BlackOverlayImage.gameObject.SetActive(false);
    }

    public void ShowObjectInfoScreen(Transform storeObjectTransform)
    {
        StockItem storedStockItem = storeObjectTransform.GetComponent<ItemStockContainer>().InventoryItem;

        //if (storedStockItem == null)
        //{
        //    ShowMessage("Object Info", storeObjectTransform.name + "\n" + "Empty");
        //}
        //else
        //{
        //    ShowMessage("Object Info", storeObjectTransform.name + "\n" + storedStockItem.Name + " " + storedStockItem.Amount + " / " + storedStockItem.MaxAmount + "\n" + storedStockItem.Category);
        //}

        //Fill info about object into window

        //Text
        Transform objectInfoTransform = WindowObjectInfo.transform;
        objectInfoTransform.Find("Title/txtTitle").GetComponent<Text>().text = storeObjectTransform.name;
        objectInfoTransform.Find("Description/txtDescription").GetComponent<Text>().text = storeObjectTransform.GetComponent<PlaceableObject>().Description;

        //Item slot
        Transform itemSlotTransform = objectInfoTransform.Find("ItemSlots/ItemSlot");

        if (storedStockItem != null) //Only if there's an item in the slot
        {
            itemSlotTransform.FindChild("TxtItemStatus").GetComponent<Text>().text = storedStockItem.Name + "\n" + storedStockItem.Amount + "/" + storedStockItem.MaxAmount;
            itemSlotTransform.FindChild("Image").GetComponent<Image>().enabled = true;
            itemSlotTransform.FindChild("Image").GetComponent<Image>().sprite = StockItemManager.Singleton.GetInventorySprite(storedStockItem.SpriteName);
            itemSlotTransform.FindChild("Radial").GetComponent<Image>().fillAmount = storedStockItem.Amount / (float)storedStockItem.MaxAmount;
        }
        else
        {
            itemSlotTransform.FindChild("TxtItemStatus").GetComponent<Text>().text = "Empty";
            itemSlotTransform.FindChild("Image").GetComponent<Image>().enabled = false;
            itemSlotTransform.FindChild("Radial").GetComponent<Image>().fillAmount = 1f;
        }

        WindowObjectInfo.SetActive(true);
    }

    public void SetSaveGameIcon(bool enabled)
    {
        SaveGameIcon.SetActive(enabled);
    }

    public void OnWeatherUpdated()
    {
        switch (EconomyManager.Singleton.CurrentWeather)
        {
            case EconomyManager.Weather.Comfortable:
                ImgTemperature.sprite = WeatherComfortable;
                break;
            case EconomyManager.Weather.Cold:
                ImgTemperature.sprite = WeatherCold;
                break;
            case EconomyManager.Weather.Hot:
                ImgTemperature.sprite = WeatherHot;
                break;
        }
    }

    public void SetFastMode(bool enabled)
    {
        if (enabled)
        {
            StoreManager.Singleton.TimeScale = 3f;
        }
        else
        {
            StoreManager.Singleton.TimeScale = 1f;
        }
    }

    public void SetSound(bool on)
    {
        SaveGameManager.SoundEnabled = on;
        SoundManager.Singleton.PlaySounds = on;

        SaveGameManager.Singleton.SavePreferences();
    }

    public void SetMusic(bool on)
    {
        SaveGameManager.MusicEnabled = on;
        SaveGameManager.Singleton.SavePreferences();

        if (on)
        {
            MusicManager.Singleton.ResumeMusic();
        }
        else
        {
            MusicManager.Singleton.StopMusic();
        }
    }

    public void ShowGameOverScreen()
    {
        GameOverScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        GameOverScreen.GetComponent<Image>().DOFade(1f, 8f);
        GameOverScreen.SetActive(true);
    }

    public void SetOptionsToggles(bool music, bool sound)
    {
        ToggleMusic.isOn = music;
        ToggleSound.isOn = sound;
    }

    public void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level, true);

        AdjustQualitySettings();

        SaveGameManager.Singleton.SavePreferences();
    }

    private void AdjustQualitySettings()
    {
#if UNITY_ANDROID
        //Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 1;
        QualitySettings.antiAliasing = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DirLight.shadows = LightShadows.None;

        if (QualitySettings.GetQualityLevel() == 0)
        {
            Camera.main.GetComponent<Antialiasing>().enabled = false;
        }
        else
        {
            Camera.main.GetComponent<Antialiasing>().enabled = true;
        }

        //if (QualitySettings.GetQualityLevel() == 0 || QualitySettings.GetQualityLevel() == 1)
        //{
        //    DirLight.shadows = LightShadows.None;
        //}
        //else
        //{
        //    DirLight.shadows = LightShadows.Hard;
        //}
#endif
    }

    private void SetQualityToggle()
    {
        switch (QualitySettings.GetQualityLevel())
        {
            case 0:
                ToggleQualityFast.isOn = true;
                break;
            case 1:
                ToggleQualityGood.isOn = true;
                break;
            case 2:
                ToggleQualityBeautiful.isOn = true;
                break;
            case 3:
                ToggleQualityFantastic.isOn = true;
                break;
        }

    }

    public void EnableControlsPanel(bool enable)
    {
        ControlsPanel.gameObject.SetActive(enable);
    }


    public void LoadGameDataToSlots()
    {
        GameSaveData data1 = SaveGameManager.Singleton.GetGameSaveDataFromSlot(1);
        GameSaveData data2 = SaveGameManager.Singleton.GetGameSaveDataFromSlot(2);
        GameSaveData data3 = SaveGameManager.Singleton.GetGameSaveDataFromSlot(3);

        SetSaveData(SaveSlot1, data1);
        SetSaveData(SaveSlot2, data2);
        SetSaveData(SaveSlot3, data3);
    }

    private void SetSaveData(Transform slotTransform, GameSaveData data)
    {
        if (data == null)
        {
            slotTransform.FindChild("txtStoreName").GetComponent<Text>().text = "";
            slotTransform.FindChild("txtDate").GetComponent<Text>().text = "";
            slotTransform.FindChild("txtMoney").GetComponent<Text>().text = "";
            slotTransform.FindChild("txtSlotEmpty").GetComponent<Text>().text = "- Empty -";
        }
        else
        {
            slotTransform.FindChild("txtStoreName").GetComponent<Text>().text = data.StoreName;
            slotTransform.FindChild("txtDate").GetComponent<Text>().text = "Y" + data.Year + " D" + data.Day + " " + data.Season;
            slotTransform.FindChild("txtMoney").GetComponent<Text>().text = data.Money.ToString("C");
            slotTransform.FindChild("txtSlotEmpty").GetComponent<Text>().text = "";
            slotTransform.GetComponent<Image>().color = data.StoreColor.GetColor();
        }
    }

    public void HideInterface()
    {
        BottomPanel.SetActive(false);
        InfoButton.SetActive(false);
        OptionsButton.SetActive(false);
        SpeedPanel.SetActive(false);
    }
}