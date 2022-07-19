using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobMainMenuCamera : MonoBehaviour
{
    private float t;
    private float Freq = 8f;
    private float amp = 5f;
    private float rotationZ;
    private float fov;
    private float baseFOV;
    private float fovChange;

    // Start is called before the first frame update
    void Start()
    {
        baseFOV = GetComponent<Camera>().fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        // I added this because I really wanted too and alose
        // it adds some personality to the game.
        t += Time.deltaTime;

        rotationZ = Mathf.Cos(t * Freq) * amp;
        fov = Mathf.Sin(t * Freq * 2f) * amp;
    
        fovChange = baseFOV + fov;

        GetComponent<Camera>().fieldOfView = fovChange;
        transform.localRotation = Quaternion.Euler(22.172f, -41.615f, rotationZ);
    }
}
