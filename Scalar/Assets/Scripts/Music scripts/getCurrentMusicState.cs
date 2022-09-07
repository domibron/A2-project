using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getCurrentMusicState : MonoBehaviour
{
    private bool isMusicMutedhold;
    private bool canIOperate = false; // this is to allow the script to run only if save manager has loaded
    private float musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        if (Save_Manager.instance.hasLoaded)
        {
            isMusicMutedhold = Save_Manager.instance.saveData.isMusicMuted;
            musicVolume = Save_Manager.instance.saveData.masterVolume;
            canIOperate = true;
        }
        else
        {
            isMusicMutedhold = false;
            musicVolume = 1f;
            canIOperate = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canIOperate)
        {
            isMusicMutedhold = Save_Manager.instance.saveData.isMusicMuted;

            musicVolume = Save_Manager.instance.saveData.masterVolume;

            AudioSource Music = GetComponent<AudioSource>();

            if (isMusicMutedhold) Music.volume = 0;
            else Music.volume = musicVolume;

        }
    }
}
