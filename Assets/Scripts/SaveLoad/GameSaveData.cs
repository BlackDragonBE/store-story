using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("GameSaveData")]
public class GameSaveData
{
    [XmlAttribute("storeName")]
    public string StoreName;

    [XmlElement("storeColor")]
    public ColorSave StoreColor;

    [XmlAttribute("money")]
    public float Money;

    [XmlAttribute("popularity")]
    public float Popularity;

    [XmlAttribute("daysBillsWerePaid")]
    public int TimesBillsWerePaid;

    [XmlAttribute("daysPlayed")]
    public int DaysPlayed;

    [XmlAttribute("day")]
    public int Day;

    [XmlAttribute("season")]
    public StoreManager.Season Season;

    [XmlAttribute("year")]
    public int Year;

    [XmlAttribute("timeHours")]
    public int StoreTimeHours;

    [XmlAttribute("timeMinutes")]
    public int StoreTimeMinutes;

    [XmlArray("Inventory"), XmlArrayItem("StockItem")]
    public List<StockItem> InventoryItems = new List<StockItem>();

    [XmlArray("StoreObjects"), XmlArrayItem("StoreObject")]
    public List<StoreObjectSave> StoreObjectSaves = new List<StoreObjectSave>();

    [XmlElement("EconomyData")]
    public EconomyData EconomyDataSave;

    [XmlArray("Upgrades"), XmlArrayItem("Upgrade")]
    public List<StoreManager.StoreUpgrade> StoreUpgrades = new List<StoreManager.StoreUpgrade>();

    public class StoreObjectSave
    {
        [XmlAttribute("positionX")]
        public float PositionX;

        [XmlAttribute("positionY")]
        public float PositionY;

        [XmlAttribute("positionZ")]
        public float PositionZ;

        [XmlAttribute("rotationX")]
        public float RotationX;

        [XmlAttribute("rotationY")]
        public float RotationY;

        [XmlAttribute("rotationZ")]
        public float RotationZ;

        [XmlAttribute("type")]
        public PlaceableObject.ObjectType ObjectType;

        [XmlAttribute("parentTile")]
        public string NameOFloorTileParent;

        [XmlElement("item")]
        public StockItem ContainedStockItem;
    }

    public class ColorSave
    {
        [XmlAttribute("R")]
        public float R;

        [XmlAttribute("G")]
        public float G;

        [XmlAttribute("B")]
        public float B;

        [XmlAttribute("A")]
        public float A;

        public ColorSave()
        {
        }

        public ColorSave(float r, float b, float g, float a)
        {
            R = r;
            B = b;
            G = g;
            A = a;
        }

        public Color GetColor()
        {
            return new Color(R, G, B, A);
        }
    }

    public class EconomyData
    {
        [XmlAttribute("1")]
        public StockItem.StockCategory TopCategory;

        [XmlAttribute("2")]
        public StockItem.StockCategory SecondCategory;

        [XmlAttribute("3")]
        public StockItem.StockCategory ThirdCategory;

        [XmlAttribute("LastChange")]
        public int DaysSinceLastChange;

        [XmlAttribute("Weather")]
        public EconomyManager.Weather Weather;

        public EconomyData()
        { }

        public EconomyData(StockItem.StockCategory topCategory, StockItem.StockCategory secondCategory, StockItem.StockCategory thirdCategory,
            int daysSinceLastChange, EconomyManager.Weather weather)
        {
            TopCategory = topCategory;
            SecondCategory = secondCategory;
            ThirdCategory = thirdCategory;

            DaysSinceLastChange = daysSinceLastChange;
            Weather = weather;
        }
    }

    public void GetAllData()
    {
        //Store
        StoreName = StoreManager.StoreName;

        StoreColor = new ColorSave
                     {
                         R = StoreManager.StoreColor.r,
                         G = StoreManager.StoreColor.g,
                         B = StoreManager.StoreColor.b,
                         A = StoreManager.StoreColor.a
                     };

        Money = StoreManager.Singleton.Money;
        DaysPlayed = StoreManager.Singleton.TotalDaysPlayed;
        Popularity = StoreManager.Singleton.StorePopularity;

        TimesBillsWerePaid = StoreManager.Singleton.TimesBillsWerePaid;

        Day = StoreManager.Singleton.Day;
        Year = StoreManager.Singleton.Year;
        Season = StoreManager.Singleton.CurrentSeason;

        StoreTimeHours = StoreManager.Singleton.StoreTime.Hours;
        StoreTimeMinutes = StoreManager.Singleton.StoreTime.Minutes;

        InventoryItems.Clear();
        InventoryItems.AddRange(StoreManager.Singleton.Inventory.ToArray());

        //Store Objects
        StoreObjectSaves.Clear();
        foreach (var so in StoreObjectManager.Singleton.StoreObjects)
        {
            StoreObjectSave newSo = new StoreObjectSave();
            newSo.PositionX = so.transform.position.x;
            newSo.PositionY = so.transform.position.y;
            newSo.PositionZ = so.transform.position.z;

            newSo.RotationX = so.transform.rotation.eulerAngles.x;
            newSo.RotationY = so.transform.rotation.eulerAngles.y;
            newSo.RotationZ = so.transform.rotation.eulerAngles.z;

            newSo.NameOFloorTileParent = so.transform.parent.name;

            newSo.ContainedStockItem = so.GetComponent<ItemStockContainer>().InventoryItem;

            StoreObjectSaves.Add(newSo);
        }

        //Economy
        EconomyDataSave = new EconomyData(EconomyManager.Singleton.TopCategory, EconomyManager.Singleton.SecondCategory, EconomyManager.Singleton.ThirdCategory, EconomyManager.Singleton.DaysSinceLastChange, EconomyManager.Singleton.CurrentWeather);

        //Upgrades
        StoreUpgrades.Clear();
        foreach (var upgrade in StoreManager.Singleton.ActiveUpgrades)
        {
            StoreUpgrades.Add(upgrade);
        }
    }

    public void ApplyDataToGame()
    {
        //Store
        StoreManager.StoreName = StoreName;

        StoreManager.StoreColor = StoreColor.GetColor();
        StoreManager.Singleton.WallMaterial.color = StoreManager.StoreColor;

        StoreManager.Singleton.Money = Money;
        StoreManager.Singleton.TotalDaysPlayed = DaysPlayed;
        StoreManager.Singleton.StorePopularity = Popularity;

        StoreManager.Singleton.TimesBillsWerePaid = TimesBillsWerePaid;

        StoreManager.Singleton.Day = Day;
        StoreManager.Singleton.Year = Year;
        StoreManager.Singleton.CurrentSeason = Season;

        StoreManager.Singleton.StoreTime = new TimeSpan(StoreTimeHours, StoreTimeMinutes, 0);

        StoreManager.Singleton.Inventory.Clear();
        StoreManager.Singleton.Inventory.AddRange(InventoryItems.ToArray());

        //Store Objects
        foreach (var storeObject in StoreObjectManager.Singleton.StoreObjects)
        {
            storeObject.transform.parent.GetComponent<FloorTile>().CanPlaceObjectHere = true;
            GameObject.Destroy(storeObject);
        }

        StoreObjectManager.Singleton.StoreObjects.Clear();

        foreach (var slot in UIManager.Singleton.ItemSlots)
        {
            GameObject.Destroy(slot);
        }

        UIManager.Singleton.ItemSlots.Clear();

        foreach (var so in StoreObjectSaves)
        {
            StoreObjectManager.Singleton.PlaceObject(so.ObjectType, new Vector3(so.PositionX, so.PositionY, so.PositionZ), Quaternion.Euler(so.RotationX, so.RotationY, so.RotationZ), so.ContainedStockItem, so.NameOFloorTileParent);
        }

        //Economy
        EconomyManager.Singleton.TopCategory = EconomyDataSave.TopCategory;
        EconomyManager.Singleton.SecondCategory = EconomyDataSave.SecondCategory;
        EconomyManager.Singleton.ThirdCategory = EconomyDataSave.ThirdCategory;
        EconomyManager.Singleton.DaysSinceLastChange = EconomyDataSave.DaysSinceLastChange;
        EconomyManager.Singleton.CurrentWeather = EconomyDataSave.Weather;

        //Upgrades
        StoreManager.Singleton.ActiveUpgrades.Clear();
        StoreManager.Singleton.ActiveUpgrades.AddRange(StoreUpgrades.ToArray());
    }
}