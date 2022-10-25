using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCam : MonoBehaviour
{
    public GameObject cover;
    public Camera inCam;
    public Camera outCam;
    // Start is called before the first frame update
    void Start()
    {
        cover.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        inCam.aspect = ((float)Screen.width) / Screen.height;
        outCam.aspect = ((float)Screen.width) / Screen.height;
    }
}
