using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private TMP_Text UItext;
    private string placeHolderText = "Sample Text";
    private List<string> TutorialStrings = new List<string>();
    private bool detectedInput = false;
    private int currentStage = 0;

    public int triggerCount = 0;

    // movemnt
    private float mouseX;
    private float mouseY;
    private float horizontalMovement;
    private float verticalMovement;

    // Start is called before the first frame update
    void Start()
    {
        UItext = GetComponentInChildren<TMP_Text>();
        // loads all the tutial lines of strings into the list
        SetListOfText();

        StartCoroutine(AnimateText());
    }

    // Update is called once per frame
    void Update()
    {
        UItext.text = placeHolderText;

        // insert some magic about skipping the text or hide it please.

        switch (currentStage)
        {
            case 0: // null / nutral value
                    // do nothing
                break;

            case 1: // mouse input detection
                mouseX = Input.GetAxisRaw("Mouse X"); // this gets the X (left and right) mouse input
                mouseY = Input.GetAxisRaw("Mouse Y"); // this gets the Y (up and down) mouse input
                break;
            case 2: // WASD input detection
                horizontalMovement = Input.GetAxisRaw("Horizontal");
                verticalMovement = Input.GetAxisRaw("Vertical");
                break;
        }
    }

    IEnumerator AnimateText()
    {
        placeHolderText = TutorialStrings[0];
        yield return new WaitForSeconds(2.2f);

        placeHolderText = TutorialStrings[1];
        yield return new WaitForSeconds(2.7f);

        // mouse
        placeHolderText = TutorialStrings[2];
        currentStage = 1; // activates mouse detection

        while (!detectedInput)
        {
            if (mouseX != 0 || mouseY != 0)
            {
                detectedInput = true;
                yield return new WaitForSeconds(0.2f);
            }
            else yield return null;
        }
        currentStage = 0; // deactivates movement detection
        detectedInput = false; // resets detected movement boolean

        placeHolderText = TutorialStrings[3];
        yield return new WaitForSeconds(2f);

        // w a s d
        placeHolderText = TutorialStrings[4];
        currentStage = 2; // activates W, A, S, D detection

        while (!detectedInput)
        {
            if (verticalMovement != 0 || horizontalMovement != 0)
            {
                detectedInput = true;
                yield return new WaitForSeconds(0.2f);
            }
            else yield return null;
        }
        currentStage = 0; // deactivates movement detection
        detectedInput = false; // resets detected movement boolean

        placeHolderText = TutorialStrings[3];
        yield return new WaitForSeconds(2f);

        // jumping
        placeHolderText = TutorialStrings[5];
        currentStage = 0; // there is no jumping detection so its disabled.

        while (!detectedInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                detectedInput = true;
                yield return new WaitForSeconds(0.2f);
            }
            else yield return null;
        }
        currentStage = 0; // deactivates movement detection
        detectedInput = false; // resets detected movement boolean

        placeHolderText = TutorialStrings[3];
        yield return new WaitForSeconds(2f);

        placeHolderText = TutorialStrings[6];
    }

    private void SetListOfText()
    {
        TutorialStrings.Add("Welcome to the tutorial!"); // 0 lists always start at 0
        TutorialStrings.Add("This will show you how to play the game."); // 1
        TutorialStrings.Add("You can look around by using the mouse."); // 2
        TutorialStrings.Add("Good!"); // 3 - Unique meanning you can reuse
        TutorialStrings.Add("You can move around the map by using the WASD keys."); // 4
        TutorialStrings.Add("You can jump by pressing the space bar."); // 5
        TutorialStrings.Add("You can enter the next room."); // 6
    }
}
