using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBLights : MonoBehaviour
{
    float time;
    float cooldown = 0f;
    [Range(0.1f, 10f)] public float coolDownTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        Light light = GetComponent<Light>();

        light.intensity = 0.8f;

        if (time >= cooldown)
        {
            if (light.color == Color.red)
            {
                light.color = Color.green;
            }
            else if (light.color == Color.green)
            {
                light.color = Color.blue;
            }
            else
            {
                light.color = Color.red;
            }

            cooldown = time + coolDownTime;
        }
    }
}
