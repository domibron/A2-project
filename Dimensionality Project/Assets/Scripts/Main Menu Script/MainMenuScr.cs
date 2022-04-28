using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuScr : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // temp timer
        {
            Application.Quit();
        }
    }

    public void LoadMap001()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadMap002()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OutErrorNotFound()
    {
        Debug.LogError("Unable to load. Reason: Missing component");
    }
}
