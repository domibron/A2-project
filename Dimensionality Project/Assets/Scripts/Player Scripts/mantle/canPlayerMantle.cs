using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canPlayerMantle : MonoBehaviour
{
    public GameObject TopCheck;
    public GameObject BottomCheck;
    private bool topCheck = false;
    private bool bottomCheck = false;
    public bool canMantle { get; private set; }

    void Start()
    {
        
    }


    void Update()
    {
        topCheck = TopCheck.GetComponent<mantleCheck>().isTriggered;
        bottomCheck = BottomCheck.GetComponent<mantleCheck>().isTriggered;
        
        if (topCheck && bottomCheck)
        {
            canMantle = false;
        }
        else if (!topCheck && bottomCheck)
        {
            canMantle = true;
        }
        else if (topCheck && !bottomCheck)
        {
            canMantle = false;
        }
        else
        {
            canMantle = false;
        }
    }
}
