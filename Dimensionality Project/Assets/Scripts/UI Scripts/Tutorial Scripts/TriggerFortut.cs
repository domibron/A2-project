using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerFortut : MonoBehaviour
{
    private Tutorial tutorialScript;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // insert int increase on the tutorial, trigger bool (optional) and disable the trigger to pervent reavtivation
            tutorialScript.triggerCount++;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            print(tutorialScript.triggerCount);
            Destroy(this);
        }
    }


    void Start()
    {
        tutorialScript = TutorialScriptReturn();
    }


    // Functions to get the script
    private Tutorial TutorialScriptReturn()
    {
        Scene currentGameScene;
        Tutorial tutorialReturn = null;

        // This is to set the base scene you are using so it can find the correct object.
        if (SceneManager.sceneCount > 1) currentGameScene = SceneManager.GetSceneAt(1);
        else currentGameScene = SceneManager.GetActiveScene();

        // This gets the script from the correct scene.
        foreach (GameObject x in currentGameScene.GetRootGameObjects())
        {
            if (x.transform.name == "Tutorial Overide")
            {
                tutorialReturn = x.transform.Find("Tutorial to Camera").GetComponent<Tutorial>();
            }
        }

        return tutorialReturn; // need to catch if null - never should happen.
    }
}
