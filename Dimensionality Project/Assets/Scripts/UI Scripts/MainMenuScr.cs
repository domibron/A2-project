using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuScr : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject levelSelection;
    public GameObject confermation;

    public Dropdown FullScreenDropdown;
    public TMP_Text LevelVBestTimeText;
    public Toggle MuteMusic;
    FullScreenMode settingVid;
    public Slider MusicVolume;
    public TMP_Text MusicVolumeText;
    public float masterVolumeValue;
    private string LevelVbestTime;
    bool shutdown = false;



    //public Scene[] AllOpenScenes;
    public Scene MasterScene;
    GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.sceneCount <= 1) // forces the master scene to load if only this scene exsists
        {
            print("eh? loading correct scene!");
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }

        foreach (Scene x in loadedScenes)
        {
            if (x.name == "Master Scene") MasterScene = x; GM = GetComponentInChildren<GameManager>();
        }

        foreach (GameObject x in MasterScene.GetRootGameObjects())
        {
            if (x.transform.name == "Game manager") GM = x.GetComponent<GameManager>();
        }

        //foreach (GameObject x in MasterScene.GetRootGameObjects())
        //{
        //    if (x.transform.name == "Game manager") GM = x.GetComponent<GameManager>();
        //}

        if (Save_Manager.instance.hasLoaded)
        {
            FullScreenDropdown.value = Save_Manager.instance.saveData.fullscreenMode;

            LevelVbestTime = Save_Manager.instance.saveData.levelVBestTime;

            MuteMusic.isOn = Save_Manager.instance.saveData.isMusicMuted;

            masterVolumeValue = Save_Manager.instance.saveData.masterVolume;
            MusicVolume.value = Save_Manager.instance.saveData.masterVolume * 100f;

            shutdown = false;
        }
        else
        {
            Save_Manager.instance.saveData.fullscreenMode = 4;

            Save_Manager.instance.saveData.levelVBestTime = "0:00.00";

            Save_Manager.instance.saveData.isMusicMuted = false;

            Save_Manager.instance.saveData.masterVolume = 1f;
            MusicVolume.value = 100f;

            shutdown = true; //  why is this nessary??
        }

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        levelSelection.SetActive(false);
        confermation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Screen.fullScreenMode = settingVid;

        masterVolumeValue = MusicVolume.value / 100f;
        MusicVolumeText.text = "Volume: " + MusicVolume.value;

        //other funtions that need to run constantly
        FullScreen();
        //Resolution();

        if (!shutdown) // this is to pervent the game from loading the best time if there is no data to load.
        {
            LevelVbestTime = Save_Manager.instance.saveData.levelVBestTime;
            LevelVBestTimeText.text = "Best time: " + LevelVbestTime;
        }
    }

    //sets the fullscreen mode depending on the dropbox value
    public void FullScreen()
    {
        switch (FullScreenDropdown.value)
        {
            case 0:
                settingVid = FullScreenMode.Windowed;
                break;

            case 1:
                settingVid = FullScreenMode.MaximizedWindow;
                break;

            case 2:
                settingVid = FullScreenMode.FullScreenWindow;
                break;

            case 3:
                settingVid = FullScreenMode.ExclusiveFullScreen;
                break;
        }
    }

    //set the resolution for the screen depending of the dropbox value.
    public void Resolution()
    {
        //switch (VideoResolution.value)
        //{
        //    case 0:
        //        Screen.SetResolution(4096, 2160, settingVid, 60);
        //        break;

        //    case 1:
        //        Screen.SetResolution(3840, 2160, settingVid, 60);
        //        break;

        //    case 2:
        //        Screen.SetResolution(2048, 1152, settingVid, 60);
        //        break;

        //    case 3:
        //        Screen.SetResolution(1920, 1080, settingVid, 60);
        //        break;

        //    case 4:
        //        Screen.SetResolution(1280, 720, settingVid, 60);
        //        break;

        //    case 5:
        //        Screen.SetResolution(640, 480, settingVid, 60);
        //        break;
        //}
    }


    //when called all the user's data will be whiped and reset
    public void DeleteAllUserData()
    {
        Save_Manager.instance.DeleteSaveData();

        Save_Manager.instance.saveData.fullscreenMode = 4;

        Save_Manager.instance.saveData.levelVBestTime = "0:00.00";

        Save_Manager.instance.saveData.levelVbestMinutes = 0;

        Save_Manager.instance.saveData.leveLVbestSeconds = 0f;

        Save_Manager.instance.saveData.levelVbestMilliseconds = 0f;

        Save_Manager.instance.saveData.levelVNewTime = true;

        Save_Manager.instance.saveData.isMusicMuted = false;

        Save_Manager.instance.saveData.masterVolume = 1f;

        MusicVolume.value = 100f;

        MuteMusic.isOn = false;

        Save_Manager.instance.Save();

        LoadSettings();
    }


    //save values after the apply button is clicked
    public void SaveSettings()
    {
        //save the values that are set

        Save_Manager.instance.saveData.fullscreenMode = FullScreenDropdown.value;

        Save_Manager.instance.saveData.isMusicMuted = MuteMusic.isOn;

        Save_Manager.instance.saveData.masterVolume = masterVolumeValue;

        //Save_Manager.instance.saveData.ScreenResolution = VideoResolution.value;

        //force save
        Save_Manager.instance.Save();
    }

    //loads the settings if the settings have been changed before.
    public void LoadSettings()
    {
        //force load the current values
        Save_Manager.instance.Load();

        //save values

        masterVolumeValue = Save_Manager.instance.saveData.masterVolume;
        MusicVolume.value = Save_Manager.instance.saveData.masterVolume * 100f;

        FullScreenDropdown.value = Save_Manager.instance.saveData.fullscreenMode;

        //VideoResolution.value = Save_Manager.instance.saveData.ScreenResolution;

        //highScore = Save_Manager.instance.saveData.HighScore;
    }

    // functions for the UI buttons

    // level loading functions and other bits
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevelV()
    {
        GM.LoadLevelV();

        //SceneManager.LoadScene(1, LoadSceneMode.Single); 
        // removed as there is a scene manager, running this will make a child scene and fuck things over also will cause crashing and lots of bugs.
    }

    public void LoadTutorialMap()
    {
        GM.Load2();

        //SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void LoadLevelOne()
    {
        GM.Load3();

        //SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void LoadDevmap()
    {
        GM.Load4();

        //SceneManager.LoadScene(4, LoadSceneMode.Single);
    }
}
