using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Jump_pad_audio : MonoBehaviour
{
    public AudioSource Jump_pad_spring;

    public void playJump_pad_spring()
    {
        Jump_pad_spring.Play();

    }


    private void OnTriggerEnter()
    {
        Jump_pad_spring.Play();
    }



}
