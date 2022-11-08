using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightScript : MonoBehaviour
{
    public float startingFlashlightBattery = 50;
    [SerializeField]
    private float flashlightBattery;
    public bool flashlightIsOn = true;
    private bool flashlightWason = true;
    public GameObject visionCone;
    private bool isThrowingFlashlight = false;

    [SerializeField]
    private FieldOfView FOV;



    // Start is called before the first frame update
    void Start()
    {
        flashlightBattery = startingFlashlightBattery;
        FOV.viewDistance = 22;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlightBattery > 22)
        {
            FOV.viewDistance = 22;
        }
        else
        {
            FOV.viewDistance = flashlightBattery;
        }

        if (FOV.flashlightReady == true && flashlightBattery > 0 && !(FOV.totalFlashlights <= 0) )
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleFlashlight();
            }
        }
        if (flashlightIsOn)
        {
            visionCone.SetActive(true);
            flashlightBattery -= 2 * Time.deltaTime;
        }
        else
        {
            visionCone.SetActive(false);
        }
        if (flashlightIsOn && flashlightBattery <= 0)
            ToggleFlashlight();
        if (flashlightIsOn && (FOV.totalFlashlights <= 0))
            ToggleFlashlight();
        

            if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FOV.totalFlashlights += 1;
        }
        CheckInputForAttack();
        
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

    void CheckInputForAttack()
    {
        if (FOV.flashlightReady == false && !isThrowingFlashlight)
        {
            flashlightWason = false;
            isThrowingFlashlight = true;
            if (flashlightIsOn)
            {
                flashlightWason = true;
                ToggleFlashlight();
            }
            flashlightBattery = startingFlashlightBattery;
        }
        if (isThrowingFlashlight && FOV.flashlightReady)
        {
            if (flashlightWason && FOV.flashlightReady == true)
            {
                ToggleFlashlight();
            }
            isThrowingFlashlight = false;
        }
    }
}
