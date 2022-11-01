using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightScript : MonoBehaviour
{

    public float flashlightBattery = 100;
    public bool flashlightIsOn = true;
    public GameObject visionCone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
        if (flashlightIsOn && flashlightBattery > 0)
        {
            visionCone.SetActive(true);
            flashlightBattery -= 10 * Time.deltaTime;
        }
        else
        {
            visionCone.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            flashlightBattery += 30;
        }

        
    }

    void ToggleFlashlight()
    {
        if (flashlightIsOn)
        {
            flashlightIsOn = false;

        }
        else
        {
            if (flashlightBattery > 0)
            { 
                flashlightIsOn = true;
            }
        }
    }
}
