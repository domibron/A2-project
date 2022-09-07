using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mantleCheck : MonoBehaviour
{
    public bool isTriggered { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        //{
        //    isTriggered = true;
        //}

        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }
}
