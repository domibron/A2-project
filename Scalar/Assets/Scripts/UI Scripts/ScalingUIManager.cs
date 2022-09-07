using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScalingUIManager : MonoBehaviour
{
    private PlayerScalingController PSC;
    private TMP_Text scaleText;

    // Start is called before the first frame update
    void Start()
    {// gets the required components
        PSC = transform.root.GetComponentInChildren<PlayerScalingController>();
        scaleText = transform.Find("Scale Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int value = PSC.currentScaleIndex;

        switch (value)
        { // obv, it checks current scale value and sets the text to what the size it
            case 0:
                scaleText.text = "1";
                break;
            case -1:
                scaleText.text = "0.5";
                break;
            case -2:
                scaleText.text = "0.25";
                break;
            case -3:
                scaleText.text = "0.125";
                break;
            default:
                scaleText.text = "error past value, not set in code!!!";
                break;
        }
    }
}




// i was here