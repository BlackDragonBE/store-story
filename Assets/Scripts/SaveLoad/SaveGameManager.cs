using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Singleton;
    public static bool AdsVersion;
    public static bool ShouldLoadGameOnStart;
    public static string SaveGameFilepathToLoad;

    public static bool SoundEnabled = true;
    public static bool MusicEnabled = true;

    public const string SettingsMusic = "music";
    public const string SettingsSound = "sound";
    public const string SettingsQuality = "quality";
    //References

    //Public
    public GameSaveData GameSaveData = new GameSaveData();
    private int _slotToSave;

    //Private

    void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        //Debug.Log("Quality: " + QualitySettings.GetQualityLevel());

        if (Application.loadedLevelName == "Store" && ShouldLoadGameOnStart)
        {
            LoadGame(SaveGameFilepathToLoad);
            ShouldLoadGameOnStart = false;
        }
    }

    public GameSaveData LoadSaveGameData(string fileName)
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath + @"\SaveGames\", fileName)))
        {
            return null;
        }

        var serializer = new XmlSerializer(typeof(GameSaveData));
        //var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", "SaveData.xml"), FileMode.Open);
        var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", fileName), FileMode.Open);
        GameSaveData = serializer.Deserialize(stream) as GameSaveData;
        stream.Close();

        if (GameSaveData != null)
        {
            return GameSaveData;
        }

        return null;
    }

    public GameSaveData GetGameSaveDataFromSlot(int slotNumber)
    {
        return LoadSaveGameData(slotNumber+ ".xml");
    }
    
    //OLD WAY, DONT USE
    //public void SaveGame()
    //{
    //    StartCoroutine(SaveRoutine());

    //    //Also save settings
    //    SavePreferences();
    //}

    public void SaveGameToSlot(int slotNumber)
    {

        _slotToSave = slotNumber;

        //Check if overwriting file
        if (GetGameSaveDataFromSlot(slotNumber) != null)
        {
            UIManager.Singleton.ShowYesNoWindow("Save to slot " + slotNumber + "?", "Are you sure you want to overwrite your save file?", ActuallySaveToSlot, null);
        }
        else
        {
            ActuallySaveToSlot();
        }
        
    }

    public void ActuallySaveToSlot()
    {

        StartCoroutine(SaveToSlotRoutine(_slotToSave));

        //Also save settings
        SavePreferences();
    }

    public void SavePreferences()
    {
        PlayerPrefsX.SetBool(SettingsMusic, MusicEnabled);
        PlayerPrefsX.SetBool(SettingsSound, SoundEnabled);
        PlayerPrefs.SetInt(SettingsQuality, QualitySettings.GetQualityLevel());
        PlayerPrefs.Save();
    }

    private IEnumerator SaveToSlotRoutine(int slotNumber)
    {
        UIManager.Singleton.SetSaveGameIcon(true);

        yield return new WaitForEndOfFrame();
        GameSaveData.GetAllData();

        Directory.CreateDirectory(Application.persistentDataPath + @"\SaveGames\");

        var serializer = new XmlSerializer(typeof(GameSaveData));
        //var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", "SaveData.xml"), FileMode.Create);
        var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", slotNumber + ".xml"), FileMode.Create);
        serializer.Serialize(stream, GameSaveData);
        stream.Close();

        //Debug.Log("Game Saved: " + Path.Combine(Application.persistentDataPath + @"\SaveGames\", "SaveData.xml"));
        Debug.Log("Game Saved: " + Path.Combine(Application.persistentDataPath + @"\SaveGames\", slotNumber + ".xml"));

        yield return new WaitForEndOfFrame();

        UIManager.Singleton.SetSaveGameIcon(false);
        UIManager.Singleton.ShowMessage("Saved", "Game saved!");
        SoundManager.Singleton.PlayGetItemSound();

    }

    /// <summary>
    /// Don't use! Old way of saving
    /// </summary>
    /// <returns></returns>
    //private IEnumerator SaveRoutine()
    //{
    //    UIManager.Singleton.SetSaveGameIcon(true);

    //    yield return new WaitForEndOfFrame();
    //    GameSaveData.GetAllData();

    //    Directory.CreateDirectory(Application.persistentDataPath + @"\SaveGames\");

    //    var serializer = new XmlSerializer(typeof(GameSaveData));
    //    //var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", "SaveData.xml"), FileMode.Create);
    //    var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", StoreManager.StoreName + ".xml"), FileMode.Create);
    //    serializer.Serialize(stream, GameSaveData);
    //    stream.Close();

    //    //Debug.Log("Game Saved: " + Path.Combine(Application.persistentDataPath + @"\SaveGames\", "SaveData.xml"));
    //    Debug.Log("Game Saved: " + Path.Combine(Application.persistentDataPath + @"\SaveGames\", StoreManager.StoreName + ".xml"));

    //    yield return new WaitForEndOfFrame();

    //    UIManager.Singleton.SetSaveGameIcon(false);

    //}

    public void LoadGame(string fileName)
    {
        var serializer = new XmlSerializer(typeof(GameSaveData));
        //var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", "SaveData.xml"), FileMode.Open);
        var stream = new FileStream(Path.Combine(Application.persistentDataPath + @"\SaveGames\", fileName), FileMode.Open);
        GameSaveData = serializer.Deserialize(stream) as GameSaveData;
        stream.Close();

        if (GameSaveData != null)
        {
            GameSaveData.ApplyDataToGame();

            MusicEnabled = PlayerPrefsX.GetBool(SettingsMusic, true);
            SoundEnabled = PlayerPrefsX.GetBool(SettingsSound, true);

            //UIManager.Singleton.SetSound(SoundEnabled);
            //UIManager.Singleton.SetMusic(MusicEnabled);

            UIManager.Singleton.SetOptionsToggles(MusicEnabled, SoundEnabled);
            EconomyManager.Singleton.JustLoadedSavedGame = true;
        }
        else
        {
            UIManager.Singleton.ShowMessage("No Saves", "No savegames found!");
        }
    }

    public string GetSaveSlotPath(int slotNumber)
    {
        return Path.Combine(Application.persistentDataPath + @"\SaveGames\", slotNumber + ".xml");
    }

    public void ApplySaveData(GameSaveData gameSaveData)
    {
        GameSaveData = gameSaveData;
        gameSaveData.ApplyDataToGame();
    }
}