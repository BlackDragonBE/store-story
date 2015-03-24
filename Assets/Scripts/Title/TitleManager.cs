using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class TitleManager : MonoBehaviour
{
    //References

    //Public
    public bool AdsVersion;
    public Transform ColorGrid;
    public Color ChosenColor;
    public Color DefaultColor;

    public Material WallMaterial;

    public Text txtStoreName;

    public YesNoDialogWindow YesNoWindow;

    //Game save
    public Transform SaveSlot1;
    public Transform SaveSlot2;
    public Transform SaveSlot3;

    //public Text txtLoadStoreName;
    //public Text txtLoadMoney;
    //public Text txtLoadDate;
    //public Text txtDebug;

    //quality
    public Toggle ToggleQualityFast;
    public Toggle ToggleQualityGood;
    public Toggle ToggleQualityBeautiful;
    public Toggle ToggleQualityFantastic;

    //public Transform SaveGameGrid;

    //public GameObject SaveGameEntryPrefab;

    public GameObject Overlay;
    public GameObject TxtAdVersion;

    public AudioSource MusicSource;


    //Private
    //List<GameSaveData> _saveGameSaves = new List<GameSaveData>();
    //List<string> _savePaths = new List<string>();

    private GameSaveData _data1;
    private GameSaveData _data2;
    private GameSaveData _data3;
    private int _saveFileToDelete;

    void Awake()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly).SetCapacity(512, 32);
        DOTween.defaultEaseType = Ease.Linear;

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(SaveGameManager.SettingsQuality, 0)); //Load quelity settings
        AdjustQualitySettings();

        if (AdsVersion)
        {
            Advertisement.Initialize("131625348");
        }
        else
        {
            TxtAdVersion.SetActive(false);
        }

    }

    private void AdjustQualitySettings()
    {
#if UNITY_ANDROID

        //Application.targetFrameRate = 30;

        QualitySettings.vSyncCount = 1;
        QualitySettings.antiAliasing = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        if (QualitySettings.GetQualityLevel() == 0)
        {
            Camera.main.GetComponent<Antialiasing>().enabled = false;
        }
        else
        {
            Camera.main.GetComponent<Antialiasing>().enabled = true;
        }

#endif
    }

    void Start()
    {
        SetQualityToggle();
        //Get saved games
        //LoadSavedGames();
        LoadGameDataToSlots();
        //GetChosenColor();
        WallMaterial.color = DefaultColor;
        ChosenColor = DefaultColor;

        //Debug.Log(_saveGameSaves.Count);
    }

    private void LoadFromIndex(string i)
    {
        int index = Int32.Parse(i);

        LoadGame(index);
    }

    public void DeleteSaveAtIndex(int i)
    {
        _saveFileToDelete = i;
        //File.Delete(_savePaths[index]);

        ShowYesNoWindow("Are you sure?", "Are you sure you want to delete your save file?", ActuallyDeleteSave, null);
        //Reload saves
        //LoadSavedGames();
        //LoadSavedGamesInGrid();
    }

    private void ActuallyDeleteSave()
    {
        File.Delete(SaveGameManager.Singleton.GetSaveSlotPath(_saveFileToDelete));
        LoadGameDataToSlots();
    }

    public void OnColorChanged()
    {
        ChosenColor = GetChosenColor();
        WallMaterial.color = ChosenColor;
    }

    private Color GetChosenColor()
    {
        foreach (var ch in ColorGrid.GetAllChildren())
        {
            if (ch.GetComponent<Toggle>().isOn)
            {
                Color col = ch.GetComponent<Image>().color;
                return col;
            }
        }

        return Color.white;
    }

    public void StartGame()
    {
        ShowOverlay();

        StoreManager.StoreColor = ChosenColor;
        StoreManager.StoreName = txtStoreName.text;
        StoreManager.AdsVersion = AdsVersion;

        Application.LoadLevelAsync("Store");
    }

    public void LoadGame(int index)
    {
        switch (index)
        {
            case 1:
                if (_data1 == null)
                {
                    return;
                }
                break;
            case 2:
                if (_data2 == null)
                {
                    return;
                }
                break;
            case 3:
                if (_data3 == null)
                {
                    return;
                }
                break;

        }

        ShowOverlay();

        StoreManager.AdsVersion = AdsVersion;
        SaveGameManager.AdsVersion = AdsVersion;

        SaveGameManager.ShouldLoadGameOnStart = true;
        //SaveGameManager.SaveGameFilepathToLoad = _savePaths[index];
        SaveGameManager.SaveGameFilepathToLoad = SaveGameManager.Singleton.GetSaveSlotPath(index);
        Application.LoadLevelAsync("Store");
    }

    public void QuitGame()
    {
        ShowYesNoWindow("Are you sure?", "Are you sure you want to quit?", QuitRoutine, null);
    }

    public void QuitRoutine()
    {
        //Makes sure editor doesn't get killed by accident
#if !UNITY_EDITOR
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }



    public void SetSound(bool on)
    {
        SoundManager.Singleton.PlaySounds = on;
        SaveGameManager.SoundEnabled = on;
        SaveGameManager.Singleton.SavePreferences();
    }

    public void SetMusic(bool on)
    {
        SaveGameManager.MusicEnabled = on;
        SaveGameManager.Singleton.SavePreferences();

        if (on)
        {
            MusicSource.Play();
        }
        else
        {
            MusicSource.Stop();
        }
    }

    public void SetQualityLevel(int level)
    {
        QualitySettings.SetQualityLevel(level, true);

        AdjustQualitySettings();

        SaveGameManager.Singleton.SavePreferences();
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

    public void ShowOverlay()
    {
        Overlay.SetActive(true);
    }


    public void LoadGameDataToSlots()
    {
        _data1 = SaveGameManager.Singleton.GetGameSaveDataFromSlot(1);
        _data2 = SaveGameManager.Singleton.GetGameSaveDataFromSlot(2);
        _data3 = SaveGameManager.Singleton.GetGameSaveDataFromSlot(3);

        SetSaveData(SaveSlot1, _data1);
        SetSaveData(SaveSlot2, _data2);
        SetSaveData(SaveSlot3, _data3);
    }

    private void SetSaveData(Transform slotTransform, GameSaveData data)
    {
        if (data == null)
        {
            slotTransform.FindChild("txtStoreName").GetComponent<Text>().text = "";
            slotTransform.FindChild("txtDate").GetComponent<Text>().text = "";
            slotTransform.FindChild("txtMoney").GetComponent<Text>().text = "";
            slotTransform.FindChild("txtSlotEmpty").GetComponent<Text>().text = "- Empty -";
            slotTransform.FindChild("btnDeleteSave").gameObject.SetActive(false);
            slotTransform.GetComponent<Image>().color = DefaultColor;
        }
        else
        {
            slotTransform.FindChild("txtStoreName").GetComponent<Text>().text = data.StoreName;
            slotTransform.FindChild("txtDate").GetComponent<Text>().text = "Y" + data.Year + " D" + data.Day + " " + data.Season;
            slotTransform.FindChild("txtMoney").GetComponent<Text>().text = data.Money.ToString("C");
            slotTransform.FindChild("txtSlotEmpty").GetComponent<Text>().text = "";
            slotTransform.FindChild("btnDeleteSave").gameObject.SetActive(true);
            slotTransform.GetComponent<Image>().color = data.StoreColor.GetColor();
        }
    }

    public void ShowYesNoWindow(string title, string question, Action onYes, Action onNo)
    {
        YesNoWindow.SetYesNoActions(onYes, onNo);
        YesNoWindow.Show(title, question);
        YesNoWindow.transform.DOScale(new Vector3(.5f, .5f, .5f), .2f).From();
    }

    public void ShareOnTwitter()
    {
        string address = "http://twitter.com/intent/tweet";

        Application.OpenURL(address +
     "?text=" + WWW.EscapeURL("Check out Store Story for Android!") +
     "&amp;url=" + WWW.EscapeURL("https://play.google.com/store/apps/details?id=storestory.be.blackdragon") +
     "&amp;related=" + WWW.EscapeURL("") +
     "&amp;lang=" + WWW.EscapeURL("en"));
    }

}