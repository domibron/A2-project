using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> music = new List<GameObject>();
    private Scene scene;
    private Scene MasterScene;
    GameManager GM;
    private AsyncOperation LoadSceneAsync;
    private int TrueCheckpointCount = 0;
    private bool isThereMasterScene = false;
    private bool isMuted;
    private AudioSource musicSource;
    private AudioSource currentMusic;
    private int x = 0;
    private bool isToStop = false;
    private bool isMutedRunOnce = false;
    private bool canIOperate;

    void Start()
    {
        // master scene instanciate
        if (SceneManager.sceneCount > 1)
        {
            int countLoaded = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];

            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            foreach (Scene x in loadedScenes)
            {
                print(x.name);
                if (x.name == "Master Scene") MasterScene = x; GM = GetComponentInChildren<GameManager>();
            }

            foreach (GameObject x in MasterScene.GetRootGameObjects())
            {
                if (x.transform.name == "Game manager") GM = x.GetComponent<GameManager>();
            }
            // end of game manager

            scene = SceneManager.GetSceneAt(1);
        }

        // gets or sets data for music
        if (Save_Manager.instance.hasLoaded)
        {
            isMuted = Save_Manager.instance.saveData.isMusicMuted;
            canIOperate = true;
        }
        else
        {
            isMuted = false;
            Save_Manager.instance.saveData.isMusicMuted = false;
            Save_Manager.instance.Save();
            canIOperate = false;
        }

        // any thing relating to music put under

        // music gathering stuff

        music.Clear();

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        
        foreach (Transform child in allChildren)
        {
            music.Add(child.gameObject);
        }

        music.RemoveAt(0); // this removes the parent from the list

        PopulateList();

        music.Sort(SortByName);

        TrueCheckpointCount = music.Count - 1;

        x = 0;

        currentMusic = music[x].GetComponent<AudioSource>();
    }

    // simple string sorting by comparing
    private static int SortByName(GameObject o1, GameObject o2)
    {
        return o1.name.CompareTo(o2.name);
    }



    // Update is called once per frame
    void Update()
    {
        if (!isMuted) isMutedRunOnce = false;

        if (isMuted)
        {
            if (!isMutedRunOnce)
            {
                currentMusic.Stop();
                isMutedRunOnce = true;
            }
            // This pervents the other if statement
            // from functioning until free like a
            // return.
        }
        else if ((!currentMusic.isPlaying) && (!isToStop))
        {
            currentMusic.Play();
            isToStop = true;
        }
        else if (!currentMusic.isPlaying)
        {
            x++;
            isToStop = false;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            NextInTrack();
        }

        if (Input.GetKeyDown(KeyCode.M)) MuteMusic();
    }

    public void MuteMusic()
    {
        isMuted = !isMuted;
        Save_Manager.instance.saveData.isMusicMuted = isMuted;
        Save_Manager.instance.Save();
    }

    public void NextInTrack()
    {
        currentMusic.Stop();
        x++;
        isToStop = false;
        if (x >= music.Count) x = 0;
        currentMusic = music[x].GetComponent<AudioSource>();
    }

    private void PopulateList()
    {
        if (LoadSceneAsync == null) return;

        if (LoadSceneAsync.isDone && music.Count == 0)
        {
            music.Clear();

            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                music.Add(child.gameObject);
            }
            music.RemoveAt(0); // this removes the parent from the list

            print(music.Count);

            for (int i = 0; i >= music.Count; i++)
            {
                print(music[i].transform.name);
            }
        }
    }

}
