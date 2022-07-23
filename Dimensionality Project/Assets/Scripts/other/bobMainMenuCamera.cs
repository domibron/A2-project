using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobMainMenuCamera : MonoBehaviour
{
    private float t;
    private float Freq = 8f;
    private float amp = 5f;
    private float rotationZ;
    private float baseRotationZ; // these might not work lol?
    private float baseRotationX;
    private float baseRotationY;
    private float fov;
    private float baseFOV;
    private float fovChange;
    private bool isTheMusicMuted;
    private Vector3 baseVector3Rotation;

    // Start is called before the first frame update
    void Start()
    {
        baseFOV = GetComponent<Camera>().fieldOfView;
        baseRotationZ = transform.localRotation.z;
        baseRotationX = transform.localRotation.x;
        baseRotationY = transform.localRotation.y;
        baseVector3Rotation = transform.localEulerAngles;

        if (Save_Manager.instance.hasLoaded)
        {
            isTheMusicMuted = Save_Manager.instance.saveData.isMusicMuted;
        }
        else
        {
            isTheMusicMuted = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isTheMusicMuted = Save_Manager.instance.saveData.isMusicMuted;

        if (isTheMusicMuted) // checks if the music is muted and has the permission.
        { // it will not change the fov or the rotation of the camera.
            //transform.rotation = Quaternion.Euler(baseRotationX, baseRotationY, baseRotationZ);
            transform.localEulerAngles = baseVector3Rotation;
            GetComponent<Camera>().fieldOfView = baseFOV;
        }
        else
        { // this bobs the camera if the music is playing.
            // I added this because I really wanted too and alose
            // it adds some personality to the game.
            t += Time.deltaTime;

            rotationZ = Mathf.Cos(t * Freq) * amp;
            fov = Mathf.Sin(t * Freq * 2f) * amp;

            fovChange = baseFOV + fov;

            GetComponent<Camera>().fieldOfView = fovChange;
            transform.localEulerAngles = new Vector3(baseVector3Rotation.x, baseVector3Rotation.y, rotationZ);
        }    
    }
}
